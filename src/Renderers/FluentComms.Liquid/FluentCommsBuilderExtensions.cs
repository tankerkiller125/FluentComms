using FluentComms.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace FluentComms.Renderers.Liquid
{
    public static class FluentCommsBuilderExtensions
    {
        public static FluentCommsBuilder UseLiquid(this FluentCommsBuilder builder)
        {
            builder.Services.AddScoped<LiquidRenderer>();
            builder.AddRenderer<LiquidRenderer>();
            return builder;
        }
    }
}
