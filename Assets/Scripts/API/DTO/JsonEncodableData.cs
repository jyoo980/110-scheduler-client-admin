using UnityEngine;
using System.Text;

/**
 * Serializes the current class into a JSON object, then to a byte array.
 */
public class JsonEncodableData {
    public byte[] GenerateRequestBodyBytes() {
        string data = JsonUtility.ToJson(this);
        return Encoding.UTF8.GetBytes(data);
    }
}
