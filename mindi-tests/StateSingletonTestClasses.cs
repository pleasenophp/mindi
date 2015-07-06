using System;
using System.Collections;
using minioc;
using minioc.attributes;
using minioc.context.bindings;
using minioc.resolution.instantiator;
using UnityEngine;
using MinDI.Objects;

public interface IAdvancedRead
{
	string tag { get; }
}

public interface IAdvancedWrite
{
	string tag { set; }
}

public class AdvancedModel : IAdvancedRead, IAdvancedWrite
{
	public string tag { get; set; }
}

public interface IDependencyTest
{
	MiniocContext context { get; }
}

public class DependencyTest : PublicContextObject, IDependencyTest
{
}