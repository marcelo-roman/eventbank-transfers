using EventBank.BuildingBlocks.Domain;
using EventBank.Transfers.Domain.Pix;

namespace EventBank.Transfers.Domain.Tests;

public sealed class PixKeyTests
{
    private static readonly Guid Ana = Guid.Parse("0199b000-0000-7000-8000-0000000000a9");
    private static readonly PixKeyOwner Owner = new(Ana, "Ana Ribeiro", "52998224725", "ana@example.com");
    private static readonly AccountReference AnaAccount = new(new AccountId(Guid.Parse("0199b000-0000-7000-8000-0000000000b1")), Ana, Currency.From("BRL"), IsOpen: true);

    [Theory]
    [InlineData(PixKeyType.Cpf, "529.982.247-25", "52998224725")]
    [InlineData(PixKeyType.Email, " Ana@Example.COM ", "ana@example.com")]
    [InlineData(PixKeyType.Phone, "(11) 98765-4321", "+5511987654321")]
    [InlineData(PixKeyType.Phone, "+55 11 98765-4321", "+5511987654321")]
    public void Keys_are_normalized(PixKeyType type, string raw, string expected) =>
        PixKeyValue.Parse(type, raw).Value.ShouldBe(expected);

    [Theory]
    [InlineData(PixKeyType.Cpf, "529.982.247-24")]
    [InlineData(PixKeyType.Email, "ana@example")]
    [InlineData(PixKeyType.Phone, "1234")]
    [InlineData(PixKeyType.Random, "not-a-guid")]
    public void Malformed_keys_are_rejected(PixKeyType type, string raw) =>
        Should.Throw<DomainException>(() => PixKeyValue.Parse(type, raw)).Code.ShouldBe("invalid_pix_key");

    [Theory]
    [InlineData("ana@example.com", PixKeyType.Email)]
    [InlineData("+5511987654321", PixKeyType.Phone)]
    [InlineData("529.982.247-25", PixKeyType.Cpf)]
    [InlineData("8f14e45f-ceea-467a-9575-8f5c2d6b8e4c", PixKeyType.Random)]
    public void The_type_of_a_typed_key_is_detected(string raw, PixKeyType expected) =>
        PixKeyValue.Detect(raw).Type.ShouldBe(expected);

    [Fact]
    public void Keys_are_masked_for_whoever_looks_them_up()
    {
        PixKeyValue.Parse(PixKeyType.Cpf, "52998224725").Masked().ShouldBe("***.982.247-**");
        PixKeyValue.Parse(PixKeyType.Email, "ana@example.com").Masked().ShouldBe("a***@example.com");
        PixKeyValue.Parse(PixKeyType.Phone, "11987654321").Masked().ShouldBe("+5511 *****-4321");
    }

    [Fact]
    public void The_same_key_always_maps_to_the_same_stream() =>
        PixKeyId.For(PixKeyValue.Parse(PixKeyType.Email, "ANA@example.com")).ShouldBe(PixKeyId.For(PixKeyValue.Parse(PixKeyType.Email, "ana@example.com")));

    [Fact]
    public void Registering_your_own_cpf_raises_PixKeyRegistered()
    {
        var key = PixKeyValue.Parse(PixKeyType.Cpf, "52998224725");

        var pixKey = PixKey.Register(key, AnaAccount, Owner, keysOnAccount: 0);

        pixKey.PendingEvents.ShouldBe([
            new PixKeyRegistered(PixKeyId.For(key).Value, "Cpf", "52998224725", AnaAccount.Id.Value, Ana, "Ana Ribeiro"),
        ]);
    }

    [Fact]
    public void Someone_else_s_cpf_cannot_be_your_key() =>
        Should.Throw<DomainException>(() => PixKey.Register(PixKeyValue.Parse(PixKeyType.Cpf, "11144477735"), AnaAccount, Owner, 0))
            .Code.ShouldBe("pix_key_not_yours");

    [Fact]
    public void A_key_cannot_point_to_another_customer_s_account() =>
        Should.Throw<DomainException>(() => PixKey.Register(PixKeyValue.NewRandom(), AnaAccount with { OwnerId = Guid.NewGuid() }, Owner, 0))
            .Code.ShouldBe("account_not_yours");

    [Fact]
    public void An_account_holds_at_most_five_keys() =>
        Should.Throw<DomainException>(() => PixKey.Register(PixKeyValue.NewRandom(), AnaAccount, Owner, PixKey.KeysPerAccount))
            .Code.ShouldBe("pix_key_limit");

    [Fact]
    public void An_active_key_cannot_be_claimed_again()
    {
        var pixKey = PixKey.Register(PixKeyValue.Parse(PixKeyType.Email, "ana@example.com"), AnaAccount, Owner, 0);

        Should.Throw<DomainException>(() => pixKey.Reclaim(AnaAccount, Owner, 1)).Code.ShouldBe("pix_key_in_use");
    }

    [Fact]
    public void A_removed_key_can_be_registered_again()
    {
        var pixKey = PixKey.Register(PixKeyValue.Parse(PixKeyType.Email, "ana@example.com"), AnaAccount, Owner, 0);
        pixKey.Remove(Ana);
        pixKey.MarkPendingEventsAsCommitted();

        pixKey.Reclaim(AnaAccount, Owner, 0);

        pixKey.Status.ShouldBe(PixKeyStatus.Active);
        pixKey.PendingEvents.Count.ShouldBe(1);
    }

    [Fact]
    public void Only_the_owner_removes_a_key()
    {
        var pixKey = PixKey.Register(PixKeyValue.NewRandom(), AnaAccount, Owner, 0);

        Should.Throw<DomainException>(() => pixKey.Remove(Guid.NewGuid())).Code.ShouldBe("pix_key_not_yours");
    }
}
