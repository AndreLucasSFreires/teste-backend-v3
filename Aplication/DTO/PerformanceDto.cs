﻿using CrossCutting;
using System.Text.Json.Serialization;

namespace Aplication.DTO
{
    public class PerformanceDto
    {
        public PerformanceDto() { }

        public Guid Id { get; set; }

        public PlayDto Play { get; set; } = new PlayDto();
        public int Audience { get; set; }

        public PlayType PlayType => Play.Type;

        public PerformanceDto(PlayDto play, int audience)
        {
            Play = play;
            Audience = audience;
        }

    }
}
