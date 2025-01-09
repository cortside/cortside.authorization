$tables = @("Subject", "Widget")

foreach ($table in $tables) {
	echo "exporting $table..."
	$sql = "SELECT * FROM Authorization.dbo.$table"
	$filename = "src\\Cortside.Authorization.WebApi.IntegrationTests\\SeedData\\$table.csv"
	Invoke-Sqlcmd -Query $sql -HostName localhost | Export-Csv -Path $filename -NoTypeInformation
}
