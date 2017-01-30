using System;
using System.Collections.Generic;

namespace Server.DTO.Telegram
{
    public class GetUpdates
    {
        public Boolean ok;
        public IList<Update> result;
    }
}
