using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPGServer
{
    public class Team
    {
        public List<UserPeer> clientPeers = new List<UserPeer>();
        public int masterRoleId = 0;

        public Team()
        {

        }

        public Team(UserPeer peer1, UserPeer peer2, UserPeer peer3)
        {
            clientPeers.Add(peer1);
            clientPeers.Add(peer2);
            clientPeers.Add(peer3);
            peer1.Team = this;
            peer2.Team = this;
            peer3.Team = this;
            masterRoleId = peer3.LoginRole.ID;//把客户端主机的角色的ID存储起来
        }

        /// <summary>
        /// 用来解散本队伍
        /// </summary>
        public void Dismiss()
        {
            masterRoleId = 0;
            foreach (UserPeer peer in clientPeers)
            {
                peer.Team = null;
            }
            clientPeers = null;
        }
    }
}
