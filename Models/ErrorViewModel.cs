namespace Models
{
    public class ErrorViewModel
    {
        private string? _requestId {get;set;}
        public string? RequestId {get{return _requestId;} set{_requestId=value;}}  

        public bool ShowRequestId => !string.IsNullOrEmpty(_requestId);
    }
}
