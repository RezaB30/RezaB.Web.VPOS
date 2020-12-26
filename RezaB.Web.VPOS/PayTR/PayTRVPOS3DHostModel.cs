using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RezaB.Web.VPOS.PayTR
{
    public class PayTRVPOS3DHostModel : VPOS3DHostModel
    {
        private const string _letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private string[] _emailServers = new[] { "@gmail.com", "@yahoo.com", "@hotmail.com", "@yandex.com", "@microsoft.com", "@mynet.com", "@live.com" };
        private ushort[] _validIPFirstPart = new ushort[]
        {
            1,
            2,
            3,
            4,
            5,
            6,
            7,
            8,
            9,
            11,
            12,
            13,
            14,
            15,
            16,
            17,
            18,
            19,
            20,
            21,
            22,
            23,
            24,
            25,
            26,
            27,
            28,
            29,
            30,
            31,
            32,
            33,
            34,
            35,
            36,
            37,
            38,
            39,
            40,
            41,
            42,
            43,
            44,
            45,
            46,
            47,
            48,
            49,
            50,
            51,
            52,
            53,
            54,
            55,
            56,
            57,
            58,
            59,
            60,
            61,
            62,
            63,
            64,
            65,
            66,
            67,
            68,
            69,
            70,
            71,
            72,
            73,
            74,
            75,
            76,
            77,
            78,
            79,
            80,
            81,
            82,
            83,
            84,
            85,
            86,
            87,
            88,
            89,
            90,
            91,
            92,
            93,
            94,
            95,
            96,
            97,
            98,
            99,
            101,
            102,
            103,
            104,
            105,
            106,
            107,
            108,
            109,
            110,
            111,
            112,
            113,
            114,
            115,
            116,
            117,
            118,
            119,
            120,
            121,
            122,
            123,
            124,
            125,
            126,
            128,
            129,
            130,
            131,
            132,
            133,
            134,
            135,
            136,
            137,
            138,
            139,
            140,
            141,
            142,
            143,
            144,
            145,
            146,
            147,
            148,
            149,
            150,
            151,
            152,
            153,
            154,
            155,
            156,
            157,
            158,
            159,
            160,
            161,
            162,
            163,
            164,
            165,
            166,
            167,
            168,
            170,
            171,
            173,
            174,
            175,
            176,
            177,
            178,
            179,
            180,
            181,
            182,
            183,
            184,
            185,
            186,
            187,
            188,
            189,
            190,
            191,
            193,
            194,
            195,
            196,
            197,
            199,
            200,
            201,
            202,
            204,
            205,
            206,
            207,
            208,
            209,
            210,
            211,
            212,
            213,
            214,
            215,
            216,
            217,
            218,
            219,
            220,
            221,
            222,
            223
        };
        private string[] _addressList = new string[] { "Adana", "Adıyaman", "Afyon", "Ağrı", "Amasya", "Ankara", "Antalya", "Artvin", "Aydın", "Balıkesir", "Bilecik", "Bingöl", "Bitlis", "Bolu", "Burdur", "Bursa", "Çanakkale", "Çankırı", "Çorum", "Denizli", "Diyarbakır", "Edirne", "Elazığ", "Erzincan", "Erzurum", "Eskişehir", "Gaziantep", "Giresun", "Gümüşhane", "Hakkari", "Hatay", "Isparta", "İçel(Mersin)", "İstanbul", "İzmir", "Kars", "Kastamonu", "Kayseri", "Kırklareli", "Kırşehir", "Kocaeli", "Konya", "Kütahya", "Malatya", "Manisa", "K.maraş", "Mardin", "Muğla", "Muş", "Nevşehir", "Niğde", "Ordu", "Rize", "Sakarya", "Samsun", "Siirt", "Sinop", "Sivas", "Tekirdağ", "Tokat", "Trabzon", "Tunceli", "Şanlıurfa", "Uşak", "Van", "Yozgat", "Zonguldak", "Aksaray", "Bayburt", "Karaman", "Kırıkkale", "Batman", "Şırnak", "Bartın", "Ardahan", "Iğdır", "Yalova", "Karabük", "Kilis", "Osmaniye", "Düzce" };
        private string _user_ip = null;
        private string _token = null;
        private string _email = null;
        private string _user_phone = null;
        private string _user_address = null;
        public PayTRVPOS3DHostModel()
        {
            merchant_oid = Guid.NewGuid().ToString().Replace("-", string.Empty);
        }
        public string merchant_salt { get; set; }
        public string token
        {
            get
            {
                if (_token != null)
                    return _token;
                _token = GetToken();
                return _token;
            }
        }
        private int merchant_id { get { return Convert.ToInt32(MerchantId); } }
        private string user_ip
        {
            get
            {
                if (_user_ip != null)
                    return _user_ip;
                _user_ip = GenerateIP();
                return _user_ip;
            }
        }
        private string merchant_oid { get; set; } // store order no max 64 char.
        public string email
        {
            get
            {
                if (_email != null)
                    return _email;
                _email = GenerateMailAddress();
                return _email;
            }
        }
        private int payment_amount { get { return Convert.ToInt32(PurchaseAmount * 100); } } // ex : for 34.56 = 34.56 * 100 = 3456
        private string currency { get { return "TRY"; } } // ex : TL or TRY , EUR , USD ...
        private string user_basket
        {
            get
            {
                object[][] user_basket = { };
                return JsonConvert.SerializeObject(user_basket);
            }
        }
        private int no_installment { get { return base.InstallmentCount == null ? 1 : 0; } } // 1 = no installment , 0 = yes ins..
        private int max_installment { get { return base.InstallmentCount == null ? 0 : base.InstallmentCount.Value; } } // set default 0 
        private string paytr_token { get { return CalculateHash(); } }
        private string user_name { get { return BillingCustomerName; } } // customer fullname max 60 char.
        public string user_address
        {
            get
            {
                if (_user_address != null)
                    return _user_address;
                _user_address = GenerateAddress();
                return _user_address;
            }
        } // customer address max 60 char.
        public string user_phone
        {
            get
            {
                if (_user_phone != null)
                    return _user_phone;
                _user_phone = GeneratePhoneNo();
                return _user_phone;
            }
        } // max 20 char.
        private string merchant_ok_url { get { return base.OkUrl; } } // redirect success page
        private string merchant_fail_url { get { return base.FailUrl; } } // redirect fail page
        private int test_mode { get { return 0; } } // for test set 1
        private int debug_on { get { return 1; } } // set 1 for errors
        //public int timeout_limit { get; set; } // if you don't send , will be 30 min.
        private string lang { get { return Language; } } // if you send empty , will be tr
        public override string ActionLink
        {
            get
            {
                return @"https://www.paytr.com/odeme/guvenli/" + token;
            }
        }

        public override string CalculateHash()
        {
            string Combine = string.Concat(MerchantId, user_ip, merchant_oid, email, payment_amount.ToString(), user_basket, no_installment.ToString(), max_installment.ToString(), currency, test_mode.ToString(), merchant_salt);
            HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(Storekey));
            byte[] b = hmac.ComputeHash(Encoding.UTF8.GetBytes(Combine));
            return Convert.ToBase64String(b);
        }

        private string GenerateIP()
        {
            var rand = new Random();
            var firstPart = _validIPFirstPart[rand.Next(_validIPFirstPart.Length)];
            return $"{firstPart}.{rand.Next(byte.MaxValue)}.{rand.Next(byte.MaxValue)}.{rand.Next(byte.MaxValue)}";
        }
        private string GenerateMailAddress()
        {
            var rand = new Random();
            var results = string.Empty;
            for (int i = 0; i < rand.Next(6, 12); i++)
            {
                results += _letters[rand.Next(_letters.Length)];
            }
            return $"{results}{_emailServers[rand.Next(_emailServers.Length)]}";
        }
        private string GeneratePhoneNo()
        {
            var rand = new Random();
            var results = "5";
            results += rand.Next(31, 56).ToString();
            for (int i = 0; i < 7; i++)
            {
                results += rand.Next(10);
            }

            return results;
        }

        private string GenerateAddress()
        {
            var rand = new Random();
            return _addressList[rand.Next(_addressList.Length)];
        }

        private string GetToken()
        {
            NameValueCollection data = new NameValueCollection();
            data.Add("currency", "TRY");
            data.Add("merchant_id", MerchantId);
            data.Add("email", email);
            data.Add("debug_on", debug_on.ToString());
            data.Add("lang", lang);
            data.Add("max_installment", max_installment.ToString());
            data.Add("merchant_oid", merchant_oid);
            data.Add("no_installment", no_installment.ToString());
            data.Add("payment_amount", payment_amount.ToString());
            data.Add("user_basket", user_basket);
            data.Add("merchant_fail_url", merchant_fail_url);
            data.Add("merchant_ok_url", merchant_ok_url);
            data.Add("test_mode", test_mode.ToString());
            //data.Add("timeout_limit", "10");
            data.Add("user_address", user_address);
            data.Add("user_ip", user_ip);
            data.Add("user_name", user_name);
            data.Add("user_phone", user_phone);
            data.Add("paytr_token", paytr_token);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            using (var client = new WebClient())
            {
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                byte[] response = client.UploadValues("https://www.paytr.com/odeme/api/get-token", "POST", data);
                string ResultAuthTicket = Encoding.UTF8.GetString(response);
                var result = JsonConvert.DeserializeObject<dynamic>(ResultAuthTicket);
                if (result.status == "success")
                {
                    return result.token;
                }
                throw new InvalidOperationException($"Error getting token. {result}");
            }
        }
    }
}
