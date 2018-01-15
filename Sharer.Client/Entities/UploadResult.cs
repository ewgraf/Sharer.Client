namespace Sharer.Client.Entities {
	public class UploadResult {
		public string Link  { get; set; }
		public string Error { get; set; }

		public bool IsSuccess => Error == null;

		public override string ToString() => $"link: {Link}, error: {Error}";
	}
}
