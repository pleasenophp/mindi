.DEFAULT_GOAL := build
include config.mk

restore:
	mono nuget.exe restore

rebuild:
	xbuild /t:rebuild mindi-core.sln

build:
	xbuild /t:build mindi-core.sln

test:
	$(NUNIT) mindi-tests/bin/Debug/mindi-tests.dll

install:
	./deploy.sh

