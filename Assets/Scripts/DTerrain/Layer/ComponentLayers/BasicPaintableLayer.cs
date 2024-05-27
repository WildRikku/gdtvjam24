namespace DTerrain
{
    public class BasicPaintableLayer : PaintableLayer<PaintableChunk>
    {
        //CHUNK SIZE X!!!!
        public virtual void Start()
        {
            SpawnChunks();
            InitChunks();
            
        }

        public virtual void Update()
        {
        }
    }
}
