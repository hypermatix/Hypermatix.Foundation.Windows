using System;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Hypermatix.Foundation.Windows.API
{
    /// <summary>
    /// Description: Helper class for P/Invoke's crypt32 api functions (not all)
    /// Last Mod: 28/5/04
    /// Author: Brendan Whelan
    /// </summary>
    public class WinCrypt
    {
        #region Native API constants

        public const uint CERT_SYSTEM_STORE_CURRENT_USER = 0x00010000;
        public const uint CERT_STORE_READONLY_FLAG = 0x00008000;
        public const uint CERT_STORE_OPEN_EXISTING_FLAG = 0x00004000;
        public const uint CERT_FIND_SUBJECT_STR = 0x00080007;
        public const uint CERT_KEY_PROV_INFO_PROP_ID = 0x00000002;
        public const uint X509_ASN_ENCODING = 0x00000001;
        public const uint PKCS_7_ASN_ENCODING = 0x00010000;
        public const uint RSA_CSP_PUBLICKEYBLOB = 19;
        public const int AT_KEYEXCHANGE = 1;  //keyspec values
        public const int AT_SIGNATURE = 2;
        public const uint DEFAULT_ENCODING_TYPE = PKCS_7_ASN_ENCODING | X509_ASN_ENCODING;

        public const uint CRYPT_EXPORTABLE = 0x00000001;
        public const uint CRYPT_USER_PROTECTED = 0x00000002;
        public const uint CRYPT_MACHINE_KEYSET = 0x00000020;
        public const uint CRYPT_USER_KEYSET = 0x00001000;
        public const uint CRYPT_DELETEKEYSET = 0x00000010;

        public const string OID_RSA_encryptedData =  "1.2.840.113549.1.7.6";
        public const string OID_RSA_DES_EDE3_CBC = "1.2.840.113549.3.7";
        public const string OID_RSA_envelopedData = "1.2.840.113549.1.7.3"; 

        #endregion

        #region Native API structures
        [StructLayout(LayoutKind.Sequential)]
        public struct CRYPT_KEY_PROV_INFO
        {
            [MarshalAs(UnmanagedType.LPWStr)]
            public String ContainerName;
            [MarshalAs(UnmanagedType.LPWStr)]
            public String ProvName;
            public uint ProvType;
            public uint Flags;
            public uint ProvParam;
            public IntPtr rgProvParam;
            public uint KeySpec;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CRYPT_ALGORITHM_IDENTIFIER
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public String pszObjId;
            public CRYPT_OBJID_BLOB Parameters;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CRYPT_OBJID_BLOB
        {
            public uint cbData;
            public IntPtr pbData;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CRYPT_DATA_BLOB
        {
            public int cbData;
            public IntPtr pbData;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PUBKEYBLOBHEADERS
        {
            public byte bType;	//BLOBHEADER
            public byte bVersion;	//BLOBHEADER
            public short reserved;	//BLOBHEADER
            public uint aiKeyAlg;	//BLOBHEADER
            public uint magic;	//RSAPUBKEY
            public uint bitlen;	//RSAPUBKEY
            public uint pubexp;	//RSAPUBKEY
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CRYPT_DECRYPT_MESSAGE_PARA
        {
            public int cbSize;
            public uint dwMsgAndCertEncodingType;
            public int cCertStore;
            public IntPtr rghCertStore;
            public uint dwFlags;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CRYPT_ENCRYPT_MESSAGE_PARA
        {
            public int cbSize;
            public uint dwMsgEncodingType;
            public int hCryptProv;
            public CRYPT_ALGORITHM_IDENTIFIER ContentEncryptionAlgorithm;
            public IntPtr pvEncryptionAuxInfo;
            public int dwFlags;
            public int dwInnerContentType;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CERT_CONTEXT
        {
            public int dwCertEncodingType;
            public IntPtr pbCertEncoded;
            public int cbCertEncoded;
            public IntPtr pCertInfo;
            public int hCertStore;
        }
#endregion

        #region Native API functions
            [DllImport("crypt32.dll", SetLastError = true)]
            public static extern bool CryptDecryptMessage(
                ref CRYPT_DECRYPT_MESSAGE_PARA pDecryptPara,
                byte[] pbEncryptedBlob,
                int cbEncryptedBlob,
                [In, Out] byte[] pbDecrypted,
                ref int pcbDecrypted,
                IntPtr ppXchgCert);

            [DllImport("crypt32.dll", SetLastError = true)]
            public static extern bool CryptEncryptMessage(
                ref CRYPT_ENCRYPT_MESSAGE_PARA pDecryptPara,
                int cRecipientCert,
                [In, MarshalAs(UnmanagedType.LPArray)] IntPtr[] rgpRecipientCert,
                byte[] pbToBeEncrypted,
                int cbToBeEncrypted,
                [In, Out] byte[] pbEncryptedBlob,
                ref int pcbEncryptedBlob);

            [DllImport("crypt32.dll")]
            public static extern bool CryptDecodeObject(
                uint CertEncodingType,
                uint lpszStructType,
                byte[] pbEncoded,
                uint cbEncoded,
                uint flags,
                [In, Out] byte[] pvStructInfo,
                ref uint cbStructInfo);


            [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool CryptAcquireContext(
                ref IntPtr hProv,
                string pszContainer,
                string pszProvider,
                uint dwProvType,
                uint dwFlags);


            //--- CryptoAPI certificate functions ------
            [DllImport("crypt32.dll", SetLastError = true)]
            public static extern IntPtr CertFindCertificateInStore(
                IntPtr hCertStore,
                uint dwCertEncodingType,
                uint dwFindFlags,
                uint dwFindType,
                [In, MarshalAs(UnmanagedType.LPWStr)]String pszFindString,
                IntPtr pPrevCertCntxt);


            [DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)] //overloaded
            public static extern IntPtr CertOpenStore(
                [MarshalAs(UnmanagedType.LPStr)] String storeProvider,
                uint dwMsgAndCertEncodingType,
                IntPtr hCryptProv,
                uint dwFlags,
                String cchNameString);


            [DllImport("crypt32.dll", SetLastError = true)]
            public static extern bool CertCloseStore(
                IntPtr hCertStore,
                uint dwFlags);

            [DllImport("crypt32.dll", SetLastError = true)]
            public static extern bool CertFreeCertificateContext(
                IntPtr hCertStore);

            [DllImport("crypt32.dll", SetLastError = true)]
            public static extern IntPtr CertEnumCertificatesInStore(
                IntPtr hCertStore,
                IntPtr pPrevCertContext);

            [DllImport("crypt32.dll", SetLastError = true)]
            public static extern bool CertGetCertificateContextProperty(
                IntPtr pCertContext, uint dwPropId, IntPtr pvData, ref uint pcbData);


            //---  CryptoAPI pfx functions ------
            [DllImport("crypt32.dll", SetLastError = true)]
            public static extern IntPtr PFXImportCertStore(
                ref CRYPT_DATA_BLOB pPfx,
                [MarshalAs(UnmanagedType.LPWStr)] String szPassword,
                uint dwFlags);

            [DllImport("crypt32.dll", SetLastError = true)]
            public static extern bool PFXVerifyPassword(
                ref CRYPT_DATA_BLOB pPfx,
                [MarshalAs(UnmanagedType.LPWStr)] String szPassword,
                uint dwFlags);

            [DllImport("crypt32.dll", SetLastError = true)]
            public static extern bool PFXIsPFXBlob(ref CRYPT_DATA_BLOB pPfx);
#endregion

    }
}
