using System;

namespace Sync.Server
{
    public class ServerImpl
    {
        //Singleton
        private static Server instance = null;

        public static ISyncServer Instance
        {
            get
            {
                if (instance == null)
                    instance = new Server();
                return instance;
            }
        }
    }
}
