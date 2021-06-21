using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationLogic
{
    public class PreRequestProcessor<TRquest> : IRequestPreProcessor<TRquest>
    {
        private readonly IServiceProvider _provider;

        public PreRequestProcessor(IServiceProvider provider)
        {
            this._provider = provider;
        }

        public async Task Process(TRquest request, CancellationToken cancellationToken)
        {
            var validatorType = GetValidatorType(typeof(ICustomValidatorType<>), typeof(TRquest));

            if (validatorType != null)
            {
                var customLogicObject = ActivatorUtilities.CreateInstance(_provider, validatorType);

                var result = (bool)validatorType.GetMethod("IsValid").Invoke(customLogicObject, new object[] { request });

                if (!result)
                {
                    throw new Exception();
                }
            }
        }

        private Type GetValidatorType(Type genericInterface, Type requestType)
        {
            return Assembly.GetAssembly(requestType)
                                .GetTypes()
                                    .FirstOrDefault(t => !t.IsGenericType && 
                                        t.GetInterfaces().Any(x => x.IsGenericType &&
                                        x.GetGenericArguments().Contains(requestType) && x.GetGenericTypeDefinition() == genericInterface));
        }
    }
}
