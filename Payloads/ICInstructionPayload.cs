using System.Collections.Generic;
using Assets.Scripts.Objects.Electrical;
using System;
using Newtonsoft.Json;

namespace StationeersWebApi.Payloads
{
    public class ICInstructionPayload
    {
        public Dictionary<string, Instruction> instructions { get; set; }

        public static ICInstructionPayload FromGame()
        {
            var item = new ICInstructionPayload();
            item.instructions = new Dictionary<string, Instruction>();

            foreach (ScriptCommand scriptCommand in Enum.GetValues(typeof(ScriptCommand)))
            {
                var instruction = Instruction.FromScriptCommand(scriptCommand);
                item.instructions[instruction.mnemonic] = instruction;
            }

            return item;
        }

        public class Instruction
        {
            [JsonIgnore]
            public string mnemonic { get; set; }
            public string description { get; set; }
            public string example { get; set; }

            public static Instruction FromScriptCommand(ScriptCommand scriptCommand)
            {
                var item = new Instruction();

                item.mnemonic = scriptCommand.ToString();
                item.description = ProgrammableChip.GetCommandDescription(scriptCommand);
                item.example = ProgrammableChip.GetCommandExample(scriptCommand);

                return item;
            }
        }
    }
}