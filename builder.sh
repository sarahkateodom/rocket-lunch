#!/bin/sh

if [ $# -eq 0 ]; then
	echo "you must choose a task"
	echo "---------------------"
	echo "build"
	echo "integration"
	# echo "feature"
	echo "unit"
	# echo "jsunit"
	echo "all"
	exit 1
fi

task="$1"

build()
{
	dotnet build ./rocket-lunch.sln
	checkError
}

# ngBuild()
# {b
# 	npm --prefix ./lp.feeservice.angular install 
# 	checkError
# 	npm --prefix ./lp.feeservice.angular run build 
# 	checkError
# }

integration() {
	build
	runIntegration
}

runIntegration() 
{
	cd ./RocketLunch.web/
	dotnet run --environment="Integration" & 
	sleep 5s && cd .. 
	dotnet test ./RocketLunch.tests/RocketLunch.tests.csproj --filter "Category=Integration"
	checkError
	killall dotnet
}

# feature() 
# {
# 	build
# 	ngBuild
# 	runFeature
# }

# runFeature()
# {
# 	npm --prefix ./lp.feeservice.angular run start-mock &
# 	sleep 5s
# 	dotnet test ./lp.feeservice.tests/lp.feeservice.tests.csproj --filter "Category=Feature"
# 	checkError
# 	lsof -ti :4201 | xargs kill -9
# }

unit() {
	build
	runUnit
}

runUnit()
{
	dotnet test ./RocketLunch.tests/RocketLunch.tests.csproj --filter "Category=Unit"
	checkError
}

# jsUnit() 
# {
# 	ngBuild
# 	runJSUnit
# }

# runJSUnit()
# {
# 	cd ./lp.feeservice.angular
# 	ng test
# 	checkError
# 	cd ..
# }

checkError()
{
	if [ $? != 0 ]; then
	    killall dotnet
		lsof -ti :4200 | xargs kill -9
		exit 1
	fi	
}

if [ $task == "all" ]; then
	build
	# ngBuild
	# runJSUnit
	# checkError
	runUnit
	checkError
	runIntegration
	# checkError
	# runFeature
	# checkError
	exit 0
fi

if [ $task == "justDoIt" ] || [ $task == "push" ] 
then
	if [ $task == "push" ]
	then
		echo JUST DO IT!!!!!!
	fi
	git pull -r && ./builder.sh all && git push && echo "ヘ( ^o^)ノ＼(^_^ )"
	exit 0
fi

eval $task