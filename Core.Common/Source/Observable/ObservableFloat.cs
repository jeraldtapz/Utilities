namespace Core.Common
{
    public class ObservableFloat : Subject<float>
    {
        private float value = 0f;
        public float Value
        {
            get => value;
            set
            {
                this.value = value;
                OnNext(value);
            }
        }
    }
}