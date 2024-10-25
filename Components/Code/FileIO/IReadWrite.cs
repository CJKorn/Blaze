//Interface so that we can use the same code to read and write files
public interface IReadWrite {
	void SerializeFile<T>(List<T> data, string filePath);
	string SerializeString<T>(List<T> data);
	List<T> DeserializeFile<T>(string filePath);
	List<T> DeserializeString<T>(string json);
}
