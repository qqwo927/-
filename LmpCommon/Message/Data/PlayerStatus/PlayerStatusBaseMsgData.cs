using Lidgren.Network;
using LmpCommon.Message.Base;
using LmpCommon.Message.Types;
using System;

namespace LmpCommon.Message.Data.PlayerStatus
{
    public abstract class PlayerStatusBaseMsgData : MessageData
    {
        /// <inheritdoc />
        internal PlayerStatusBaseMsgData() { }
        public override ushort SubType => (ushort)(int)PlayerStatusMessageType;
        public virtual PlayerStatusMessageType PlayerStatusMessageType => throw new NotImplementedException();

        internal override void InternalSerialize(NetOutgoingMessage lidgrenMsg)
        {
            //Nothing to implement here
        }

        internal override void InternalDeserialize(NetIncomingMessage lidgrenMsg)
        {
            //Nothing to implement here
        }

        internal override int InternalGetMessageSize()
        {
            return 0;
        }
    }
}
