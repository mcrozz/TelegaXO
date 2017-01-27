using System;
using System.Collections.Generic;

namespace Server.DTO.Telegram
{
    class GetUpdates
    {
        public Boolean ok;
        public IList<Update> result;
    }
}
