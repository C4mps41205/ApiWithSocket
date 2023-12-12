using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using para_casa;

namespace para_casa
{
    internal class Routes
    {
        [JsonProperty("/resposta1")]
        public Resposta1 resposta1 = new Resposta1();

        [JsonProperty("/resposta2")]
        public Resposta2 resposta2 = new Resposta2();
    }
}
