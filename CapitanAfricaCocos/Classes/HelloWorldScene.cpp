/****************************************************************************
 Copyright (c) 2017-2018 Xiamen Yaji Software Co., Ltd.

 http://www.cocos2d-x.org

 Permission is hereby granted, free of charge, to any person obtaining a copy
 of this software and associated documentation files (the "Software"), to deal
 in the Software without restriction, including without limitation the rights
 to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:

 The above copyright notice and this permission notice shall be included in
 all copies or substantial portions of the Software.

 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 THE SOFTWARE.
 ****************************************************************************/
#include "ui/CocosGUI.h"

#include "HelloWorldScene.h"

USING_NS_CC;

Scene* HelloWorld::createScene()
{
    return HelloWorld::create();
}

// Print useful error message instead of segfaulting when files are not there.
static void problemLoading(const char* filename)
{
    printf("Error while loading: %s\n", filename);
    printf("Depending on how you compiled you might have to add 'Resources/' in front of filenames in HelloWorldScene.cpp\n");
}

// on "init" you need to initialize your instance
bool HelloWorld::init()
{
    //////////////////////////////
    // 1. super init first
    if ( !Scene::init() )
    {
        return false;
    }

    auto visibleSize = Director::getInstance()->getVisibleSize();
    Vec2 origin = Director::getInstance()->getVisibleOrigin();

    /////////////////////////////////////////////////////////
/*
    auto closeItem = MenuItemImage::create(
                                           "CloseNormal.png",
                                           "CloseSelected.png",
                                           CC_CALLBACK_1(HelloWorld::menuCloseCallback, this));

    if (closeItem == nullptr ||
        closeItem->getContentSize().width <= 0 ||
        closeItem->getContentSize().height <= 0)
    {
        problemLoading("'CloseNormal.png' and 'CloseSelected.png'");
    }
    else
    {
        float x = origin.x + visibleSize.width - closeItem->getContentSize().width/2;
        float y = origin.y + closeItem->getContentSize().height/2;
        closeItem->setPosition(Vec2(x,y));
    }

    // create menu, it's an autorelease object
    auto menu = Menu::create(closeItem, NULL);
    menu->setPosition(Vec2::ZERO);
    this->addChild(menu, 1);
*/
    /////////////////////////////
    // 3. add your codes below...

    // add a label shows "Hello World"
    // create and initialize a label
/*
    auto label = Label::createWithTTF("Hello World", "fonts/Marker Felt.ttf", 24);
    if (label == nullptr)
    {
        problemLoading("'fonts/Marker Felt.ttf'");
    }
    else
    {
        // position the label on the center of the screen
        label->setPosition(Vec2(origin.x + visibleSize.width/2,
                                origin.y + visibleSize.height - label->getContentSize().height));

        // add the label as a child to this layer
        this->addChild(label, 1);
    }

 */

    // add "HelloWorld" splash screen"
    auto sprite_background = Sprite::create("bg_6.png");
    if (sprite_background == nullptr)
    {
        problemLoading("'bg_6.png'");
    }
    else
    {
        // position the sprite on the center of the screen
        sprite_background->setPosition(Vec2(visibleSize.width/2 + origin.x, visibleSize.height/2 + origin.y));
        auto sWidth = sprite_background->getContentSize().width;
        auto sHeight = sprite_background->getContentSize().height;
        sprite_background->setScale(visibleSize.width/sWidth, visibleSize.height/sHeight);
        // add the sprite as a child to this layer
        this->addChild(sprite_background, 0);
    }


    auto sprite_logo = Sprite::create("logo.png");
    if(sprite_logo == nullptr)
    {
        problemLoading("'logo.png'");

    }
    else
    {
        sprite_logo->setPosition(Vec2(visibleSize.width/2 + origin.x, visibleSize.height/2 + origin.y));

        auto spriteCalcHeight = visibleSize.height/2;
        auto spriteHeightFactor = sprite_logo->getContentSize().height/spriteCalcHeight;

        sprite_logo->setScale(1/spriteHeightFactor, 1/spriteHeightFactor);
        // add the sprite as a child to this layer
        this->addChild(sprite_logo, 0);
    }

    auto button_quit = ui::Button::create("ui/green_turn_off 1.png", "ui/Button_Press.png", "ui/Button_Disable.png");
    if(button_quit == nullptr)
    {

    }
    else
    {
        button_quit->setPosition(Vec2(visibleSize.width/6 + origin.x, visibleSize.height/5 + origin.y));

        auto buttonCalcHeight = visibleSize.height/5;
        auto buttonHeightFactor = button_quit->getContentSize().height/buttonCalcHeight;
        button_quit->setScale(1/buttonHeightFactor, 1/buttonHeightFactor);

        button_quit->addTouchEventListener(CC_CALLBACK_1(HelloWorld::menuCloseCallback, this));

        this->addChild(button_quit, 0);
    }

    auto button_play = ui::Button::create("ui/green_play.png", "ui/Button_Press.png", "ui/Button_Disable.png");
    if(button_play == nullptr)
    {

    }
    else
    {
        button_play->setPosition(Vec2(visibleSize.width/2 + origin.x, visibleSize.height/5 + origin.y));

        auto buttonCalcHeight = visibleSize.height/5;
        auto buttonHeightFactor = button_play->getContentSize().height/buttonCalcHeight;
        button_play->setScale(1/buttonHeightFactor, 1/buttonHeightFactor);

        button_play->addTouchEventListener(CC_CALLBACK_1(HelloWorld::menuCloseCallback, this));

        this->addChild(button_play, 0);
    }

    auto button_settings = ui::Button::create("ui/grey_settings.png", "ui/Button_Press.png", "ui/Button_Disable.png");
    if(button_settings == nullptr)
    {

    }
    else
    {
        button_settings->setPosition(Vec2(5*visibleSize.width/6 + origin.x, visibleSize.height/5 + origin.y));

        auto buttonCalcHeight = visibleSize.height/5;
        auto buttonHeightFactor = button_settings->getContentSize().height/buttonCalcHeight;
        button_settings->setScale(1/buttonHeightFactor, 1/buttonHeightFactor);

        button_settings->addTouchEventListener(CC_CALLBACK_1(HelloWorld::menuCloseCallback, this));

        this->addChild(button_settings, 0);
    }



    return true;
}


void HelloWorld::menuCloseCallback(Ref* pSender)
{
    //Close the cocos2d-x game scene and quit the application
    Director::getInstance()->end();

    /*To navigate back to native iOS screen(if present) without quitting the application  ,do not use Director::getInstance()->end() as given above,instead trigger a custom event created in RootViewController.mm as below*/

    //EventCustom customEndEvent("game_scene_close_event");
    //_eventDispatcher->dispatchEvent(&customEndEvent);


}
