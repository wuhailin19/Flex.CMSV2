﻿namespace Flex.Domain.Entities
{
    public class TestTranscation : EntityContext
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string Password { get; set; }
    }
}
