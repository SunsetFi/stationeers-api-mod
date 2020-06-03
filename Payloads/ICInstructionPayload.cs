using System.Collections.Generic;
using Assets.Scripts.Objects.Electrical;
using System;

namespace WebAPI.Payloads
{
    public class ICInstructionPayload
    {
        public List<Instruction> instructions { get; set; }

        public static ICInstructionPayload FromGame()
        {
            var item = new ICInstructionPayload();
            item.instructions = new List<Instruction>();

            foreach (ScriptCommand scriptCommand in Enum.GetValues(typeof(ScriptCommand))) 
            {
                item.instructions.Add(Instruction.FromScriptCommand(scriptCommand));
            }

            return item;
        }

        public class Instruction 
        {
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