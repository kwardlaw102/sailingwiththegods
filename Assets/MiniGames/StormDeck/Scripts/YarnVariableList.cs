using System.Collections.Generic;
using Yarn.Unity;

class YarnVariableList
{
	private readonly List<InMemoryVariableStorage.DefaultVariable> variableList = new List<InMemoryVariableStorage.DefaultVariable>();

	public InMemoryVariableStorage.DefaultVariable[] ToArray() {
		return variableList.ToArray();
	}

	public void Add(string name, string value) {
		Add(name, value, Yarn.Value.Type.String);
	}

	public void Add(string name, float value) {
		Add(name, value.ToString(), Yarn.Value.Type.Number);
	}

	public void Add(string name, bool value) {
		Add(name, value.ToString(), Yarn.Value.Type.Bool);
	}

	private void Add(string name, string value, Yarn.Value.Type type) {
		InMemoryVariableStorage.DefaultVariable newVariable = new InMemoryVariableStorage.DefaultVariable();
		newVariable.name = name;
		newVariable.value = value;
		newVariable.type = type;
		variableList.Add(newVariable);
	}
}
