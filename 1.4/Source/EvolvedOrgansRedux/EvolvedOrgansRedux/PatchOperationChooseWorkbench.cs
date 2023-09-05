using System.Collections.Generic;
using System.Xml;
using Verse;

namespace EvolvedOrgansRedux {
	public class PatchOperationChooseWorkbench : PatchOperation {
		private string modname = null;
		private List<PatchOperation> operations;
		private PatchOperation lastFailedOperation;

		protected override bool ApplyWorker(XmlDocument xml) {
			if (modname != null && !Singleton.Instance.settings.workbenches.Contains(modname))
				Singleton.Instance.settings.workbenches.Add(modname);

			if (Singleton.Instance.settings.ChosenWorkbench == modname) {
				foreach (PatchOperation operation in operations) {
					if (!operation.Apply(xml)) {
						lastFailedOperation = operation;
						Verse.Log.Error("EvolvedOrgansRedux: Error in PatchOperationChooseWorkbench -> " + operation.ToString());
						return false;
					}
				}
			}
			return true;
		}
		public override void Complete(string modIdentifier) {
			base.Complete(modIdentifier);
			lastFailedOperation = null;
		}

		public override string ToString() {
			int num = ((operations != null) ? operations.Count : 0);
			string text = $"{base.ToString()}(count={num}";
			if (lastFailedOperation != null)
			{
				text = text + ", lastFailedOperation=" + lastFailedOperation;
			}
			return text + ")";
		}
	}
}
