namespace CLUNL.Graph
{
    public class ConstantFloatValueNode:LogicalGraphNode
    {
        public float Value;
        public override object Execute()
        {
            return Value;
        }
    }
}
