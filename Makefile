.DEFAULT_GOAL := build
include config.mk
export

restore:
	$(MAKE) -C solution restore
rebuild:
	$(MAKE) -C solution rebuild
build:
	$(MAKE) -C solution build
test:
	$(MAKE) -C solution test

install:
ifeq ($(DEPLOY_DIR),)
	$(error Deploy dir is not set in config.mk)
endif
	@rm -f $(DEPLOY_DIR)/mindi.dll
	@rm -f $(DEPLOY_DIR)/mindi-interop.dll
	@rm -f $(DEPLOY_DIR)/mindi-unity.dll
	@rm -f $(DEPLOY_DIR)/minioc.dll
	
	@cp solution/mindi/bin/$(BUILD_TARGET)/mindi.dll $(DEPLOY_DIR)/mindi.dll
	@cp solution/mindi/bin/$(BUILD_TARGET)/mindi-interop.dll $(DEPLOY_DIR)/mindi-interop.dll
	@cp solution/mindi/bin/$(BUILD_TARGET)/minioc.dll $(DEPLOY_DIR)/minioc.dll	

install-unity: install
	@cp solution/mindi-unity/bin/$(BUILD_TARGET)/mindi-unity.dll $(DEPLOY_DIR)/mindi-unity.dll

merge-master:
	git checkout unity4 && git merge master && git push
	git checkout unity5 && git merge master && git push
	git checkout master

