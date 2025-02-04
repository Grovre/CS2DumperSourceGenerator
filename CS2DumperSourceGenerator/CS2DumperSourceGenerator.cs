using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace A2XDumperSourceGenerator
{
    [Generator]
    public class A2XDumperSourceGenerator : ISourceGenerator, IIncrementalGenerator
    {
        readonly Dictionary<string, string[]> Links = new()
        {
            {"Schemas", [
                "https://raw.githubusercontent.com/Grovre/cs2-dumper/refs/heads/main/output/worldrenderer_dll.cs",
                "https://raw.githubusercontent.com/Grovre/cs2-dumper/refs/heads/main/output/vphysics2_dll.cs",
                "https://raw.githubusercontent.com/Grovre/cs2-dumper/refs/heads/main/output/soundsystem_dll.cs",
                "https://raw.githubusercontent.com/Grovre/cs2-dumper/refs/heads/main/output/server_dll.cs",
                "https://raw.githubusercontent.com/Grovre/cs2-dumper/refs/heads/main/output/schemasystem_dll.cs",
                "https://raw.githubusercontent.com/Grovre/cs2-dumper/refs/heads/main/output/scenesystem_dll.cs",
                "https://raw.githubusercontent.com/Grovre/cs2-dumper/refs/heads/main/output/resourcesystem_dll.cs",
                "https://raw.githubusercontent.com/Grovre/cs2-dumper/refs/heads/main/output/rendersystemdx11_dll.cs",
                "https://raw.githubusercontent.com/Grovre/cs2-dumper/refs/heads/main/output/pulse_system_dll.cs",
                "https://raw.githubusercontent.com/Grovre/cs2-dumper/refs/heads/main/output/particles_dll.cs",
                "https://raw.githubusercontent.com/Grovre/cs2-dumper/refs/heads/main/output/panorama_dll.cs",
                "https://raw.githubusercontent.com/Grovre/cs2-dumper/refs/heads/main/output/networksystem_dll.cs",
                "https://raw.githubusercontent.com/Grovre/cs2-dumper/refs/heads/main/output/materialsystem2_dll.cs",
                "https://raw.githubusercontent.com/Grovre/cs2-dumper/refs/heads/main/output/host_dll.cs",
                "https://raw.githubusercontent.com/Grovre/cs2-dumper/refs/heads/main/output/engine2_dll.cs",
                "https://raw.githubusercontent.com/Grovre/cs2-dumper/refs/heads/main/output/client_dll.cs",
                "https://raw.githubusercontent.com/Grovre/cs2-dumper/refs/heads/main/output/animationsystem_dll.cs"
                ]
            },
            { "Interfaces", [
                "https://raw.githubusercontent.com/Grovre/cs2-dumper/refs/heads/main/output/interfaces.cs"
                ]
            },
            { "Offsets", [
                "https://raw.githubusercontent.com/Grovre/cs2-dumper/refs/heads/main/output/offsets.cs"
                ]
            },
            { "Buttons", [
                "https://raw.githubusercontent.com/Grovre/cs2-dumper/refs/heads/main/output/buttons.cs"
                ]
            }
        };

        public void Execute(GeneratorExecutionContext context)
        {
            Debugger.Launch();

        }

        public void Initialize(GeneratorInitializationContext context)
        {
            // Do nothing, be happy
        }

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(context2 =>
            {
                // Debugger.Launch();
                using var httpClient = new HttpClient();
                var getTasks = Links
                    .SelectMany(kv => kv.Value)
                    .Select(link => (link, httpClient.GetStringAsync(link)))
                    .ToArray();

                foreach (var (Link, SourceTask) in getTasks)
                {
                    var source = SourceTask.GetAwaiter().GetResult();
                    source = "// TEST!!!\n" + source;
                    var hintName = Link.Split('/').Last().Split('.').First();

                    context2.AddSource(hintName, source);
                }
            });
        }
    }
}
