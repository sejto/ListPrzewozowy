﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class getdane
    {
        //       const string keyname = "HKEY_CURRENT_USER\\c#Test\\PUL";
        public string ip, user, pass;
        public string dane(string keyname)
        {
            rejestrIO rejestr = new rejestrIO();
            ip = rejestr.czytajklucz(keyname, "ip", false);
            user = rejestr.czytajklucz(keyname, "user", false);
            pass = rejestr.czytajklucz(keyname, "pass", true);
            return ip;
            return user;
            return pass;
        }


    }
    class rejestrIO
    {
        const string salt = "f$4e9$#n!#98iaf542";
        public void zapiszklucz(string keyName, string valuename, string valuedata, bool crypt)
        {
            if (crypt != true)
            {
                Registry.SetValue(keyName, valuename, valuedata);
            }
            else
            {
                string valdatacrypt = Cipher.Encrypt(valuedata, salt);
                Registry.SetValue(keyName, valuename, valdatacrypt);
            }
        }
        public string czytajklucz(string keyName, string valuename, bool crypt)
        {
            if (crypt != true)
            {
                string klucz = (string)Registry.GetValue(keyName, valuename, "Value does not exist.");
                return klucz;
            }
            else
            {
                string kluczcrypt = (string)Registry.GetValue(keyName, valuename, "Value does not exist.");
                string klucz = Cipher.Decrypt(kluczcrypt, salt);
                return klucz;
            }
        }

    }

}

