using AutoBogus;
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