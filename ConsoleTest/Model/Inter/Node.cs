﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myapp.Model.Inter
{
    public class Node
    {

        [JsonProperty(Order = 1)]
        public string type { get; set; } = "Node";

        public Node(string type)
        {
            this.type = type;
        }

    }
}
