﻿using System.Text.Json.Serialization;

namespace BankRestAPI.Models
{
    public class Transfer
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        // Datos de origen
        public Guid FromBankId { get; set; } = default!;
        public string FromBankName { get; set; } = default!;
        [JsonIgnore]
        public Bank? FromBank { get; set; } = default!;
        public Guid FromAccountId { get; set; } = default!;
        [JsonIgnore]
        public Account FromAccount { get; set; } = default!;

        // Datos de destino
        public Guid ToBankId { get; set; } = default!;
        public string ToBankName { get; set; } = default!;
        [JsonIgnore]
        public Bank? ToBank { get; set; } = default!;
        public Guid ToAccountId { get; set; } = default!;
        [JsonIgnore]
        public Account ToAccount { get; set; } = default!;

    }
}
