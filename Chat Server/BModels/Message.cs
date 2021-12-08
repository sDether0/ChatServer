﻿using Newtonsoft.Json;

namespace Chat_Server.BModels
{
    public class InMessage
    {
        [JsonRequired]
        public string token { get; set; }
        [JsonRequired]
        public int toid { get; set; }
        [JsonRequired]
        public string text { get; set; }
        public string? reply { get; set; }
    }
}
