using SoapTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static class SOAPEndpointExtensions
    {
        public static IApplicationBuilder UseSOAPEndpoint<T>(this IApplicationBuilder builder, string path, Binding binding)
        {
            var encoder = binding.CreateBindingElements().Find<MessageEncodingBindingElement>()?.CreateMessageEncoderFactory().Encoder;
            return builder.UseMiddleware<SOAPEndpointMiddleware>(typeof(T), path, encoder);
        }
    }
}
