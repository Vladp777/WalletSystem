using AutoBogus;
using Bogus;
using AutoBogus.Conventions;
using Domain.Entities;

namespace DataGenerator;

public class AutoGenerator<T> where T : class
{
    AutoFaker<T> _dataFake;
    public AutoGenerator()
    {

        AutoFaker.Configure(builder =>
        {
            builder.WithConventions();
        });

        _dataFake = new AutoFaker<T>();

    }

    public T Generate()
    {
        return _dataFake.Generate();
    }

    public IEnumerable<T> Generate(int n)
    {
        return _dataFake.Generate(n);
    }
}

public class AutoAccGenerator
{
    readonly Faker<Account> _dataFake;
    public AutoAccGenerator()
    {

        AutoFaker.Configure(builder =>
        {
            builder.WithConventions();
        });

        _dataFake = new AutoFaker<Account>()
            .RuleFor(a => a.UserId, a => a.Random.Guid().ToString());



    }

    public Account Generate()
    {
        return _dataFake.Generate();
    }

    public IEnumerable<Account> Generate(int n)
    {
        return _dataFake.Generate(n);
    }
}