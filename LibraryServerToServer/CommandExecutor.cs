using System.Threading;

namespace LibraryServerToServer
{
    public class CommandExecutor : IExecutor
    {
        public object Execute(object data)
        {
            CommandExecutorHandler commandExecutorHandler = new CommandExecutorHandler((Command)data);
            Thread thread = commandExecutorHandler.Handle();
            thread.Start();
            Monitor.Wait(thread);
            return commandExecutorHandler.GetResult();
        }

        class CommandExecutorHandler
        {
            private object data;
            private Thread thread;

            public CommandExecutorHandler(Command command)
            {
                data = command;
            }

            public Thread Handle()
            {
                thread = new Thread(Handler);
                return thread;
            }

            public object GetResult()
            {
                return data;
            }

            private void Handler()
            {
                data = (int)((Command)data).GetData()[0] + (int)((Command)data).GetData()[1];
                Monitor.Pulse(thread);
            }
        }
    }
}
