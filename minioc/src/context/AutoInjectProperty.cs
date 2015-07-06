using System.Reflection;

namespace minioc.context {
internal class AutoInjectProperty : AutoInjectMember {
    private readonly PropertyInfo _propertyInfo;

    public AutoInjectProperty(PropertyInfo propertyInfo) {
        _propertyInfo = propertyInfo;
    }

    public object getValue(object instance) {
        return _propertyInfo.GetValue(instance, null);
    }

    public override string ToString() {
        return _propertyInfo.DeclaringType.Name + "." + _propertyInfo.Name;
    }

}
}