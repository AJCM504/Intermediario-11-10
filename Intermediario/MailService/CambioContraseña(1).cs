using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intermediario.MailService
{
    internal class CambioContraseña:ServidorCorreos
    {
        public CambioContraseña()
        {
            senderMail = "aloj_chavarriam@unicah.edu";
            password = "catracho";
            host = "smtp.unicah.edu";
            port = 587;
            ssl= true;
            initializaSetpCliente();
        }
    }
}
