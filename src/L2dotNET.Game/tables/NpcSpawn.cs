﻿namespace L2dotNET.GameService.Tables
{
    class NpcSpawn
    {
        private static readonly NpcSpawn ns = new NpcSpawn();

        public static NpcSpawn getInstance()
        {
            return ns;
        }
    }
}