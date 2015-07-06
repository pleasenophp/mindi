using System.Reflection;

namespace minioc.context {
internal class AutoInjectField : AutoInjectMember {
    private readonly FieldInfo _fieldInfo;

    public AutoInjectField(FieldInfo fieldInfo) {
        _fieldInfo = fieldInfo;
    }

    public object getValue(object instance) {
        return _fieldInfo.GetValue(instance);
    }

    public override string ToString() {
        return _fieldInfo.DeclaringType.Name + "." + _fieldInfo.Name;
    }
}
}