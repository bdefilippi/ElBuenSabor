﻿using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSabor.Hubs
{
    public class NotificacionesAClienteHub: Hub
    {
        public async Task JoinRolIDToGroup(long rolID)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, rolID.ToString());
        }

        public async Task RemoveRolIDFromGroup(long rolID)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, rolID.ToString());
        }


        public async Task JoinClienteIDToGroup(long clienteID)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, clienteID.ToString());
        }

        public async Task RemoveClienteIDFromGroup(long clienteID)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, clienteID.ToString());
        }

    }
}
