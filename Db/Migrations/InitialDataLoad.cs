using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Db.Migrations
{
    public partial class InitilaCreate
    {
        private void loadData(MigrationBuilder migrationBuilder) 
        {
            migrationBuilder.InsertData("r_blog", "url", new string[] { "HAha", "Haha 2" });
        }
    }
}
