using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solocast.Core.Interfaces
{
	public interface IDatabaseMigrator
	{
		void Migrate();
	}
}
