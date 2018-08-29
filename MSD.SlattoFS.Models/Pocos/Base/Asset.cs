namespace MSD.SlattoFS.Models.Pocos.Base
{
    public interface IAsset
    {
        int Id { get; set; }
        int MediaId { get; set; }
        string Url { get; set; }        
    }
}
