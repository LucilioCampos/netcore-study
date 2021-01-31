using System.Collections.Generic;
using Commander.Models;

namespace Commander.Data
{
    public class MockcCommanderRepository : ICommanderRepository
    {
        public List<Command> commands;

        public MockcCommanderRepository()
        {
            commands = new List<Command>
            {
                new Command(Id: 0, HowTo: "teste", Line: "teste", Plataform: "teste")
            };
        }
        public IEnumerable<Command> GetAllCommands()
        {
            return commands;
        }

        public Command GetCommandById(int id) => new Command(Id: 0, HowTo: "teste", Line: "teste", Plataform: "teste");

        public bool DeleteCommand(int id)
        {
            var delete = commands.RemoveAll(command => command.Id == id);

            if(delete > 0) {
                return true;
            }
            return false;
        }

        public void UpdateCommand(Command command)
        {
            throw new System.NotImplementedException();
        }

        public bool SaveChanges()
        {
            throw new System.NotImplementedException();
        }

        public void CreateCommand(Command cmd)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteCommand(Command cmd)
        {
            throw new System.NotImplementedException();
        }
    }
}