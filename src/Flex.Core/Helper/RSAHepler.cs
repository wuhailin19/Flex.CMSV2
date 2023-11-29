using Flex.Core.Helper;
using System.Reflection;

namespace Flex.Core.Admin.Application;

public class RSAHepler
{
    private const string publickey = "publickey.key";
    private const string publicxmlkey = "publickeyXml.key";
    private const string privatexmlkey = "privatekeyXml.crt";
    private const string privatekey = "privatekey.crt";
    readonly static string currentDirectory;
    static RSAHepler()
    {
        var type = Assembly.GetExecutingAssembly();
        currentDirectory = Path.GetDirectoryName(type.Location);
        if (Consts.RSAKeyMode== "new") {
            EncryptHelper.CreateJavaKeyFileForAsymmetricAlgorithm(Path.Combine(currentDirectory, publickey), Path.Combine(currentDirectory, privatekey));
            EncryptHelper.CreateCSharpKeyFileForAsymmetricAlgorithm(Path.Combine(currentDirectory, publicxmlkey), Path.Combine(currentDirectory, privatexmlkey));
        }
        RSAPublicKey= EncryptHelper.GetKeyFromFileForAsymmetricAlgorithm(Path.Combine(currentDirectory, publickey));
        RSAPublicXmlKey = EncryptHelper.GetKeyFromFileForAsymmetricAlgorithm(Path.Combine(currentDirectory, publicxmlkey));
        RSAPrivateKey = EncryptHelper.GetKeyFromFileForAsymmetricAlgorithm(Path.Combine(currentDirectory, privatekey));
        RSAPrivateXmlKey = EncryptHelper.GetKeyFromFileForAsymmetricAlgorithm(Path.Combine(currentDirectory, privatexmlkey));
    }
    public static string RSAPublicKey;
    public static string RSAPublicXmlKey;
    public static string RSAPrivateKey;
    public static string RSAPrivateXmlKey;
}
