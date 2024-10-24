//Interface so that we can use the same code to read and write files
public interface IReadWrite {
	void Serialize<T>(List<T> data, string filePath);
	List<T> Deserialize<T>(string filePath);
}
