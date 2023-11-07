/*
 * Warped Demo Code
 * @copyright    2017 Ansimuz
 * Get more free assets and code like these at: www.pixelgameart.org
 * Visit my store for premium content at https://ansimuz.itch.io/
 * */

var game;
var player;
var background;
var middleground;
var gameWidth = 240;
var gameHeight = 176;
var globalMap;
var shootingFlag;
var nextShot;
var projectiles;
var enemies;
var items;
var isDuck;
var hurtFlag;

window.onload = function () {

    game = new Phaser.Game(gameWidth, gameHeight, Phaser.AUTO, "");
    game.state.add('Boot', boot);
    game.state.add('Preload', preload);
    game.state.add('TitleScreen', titleScreen);
    game.state.add('PlayGame', playGame);
    //
    game.state.start("Boot");
}

var boot = function (game) {
};
boot.prototype = {
    preload: function () {
        this.game.load.image('loading', 'assets/sprites/loading.png');
    },
    create: function () {
        game.scale.pageAlignHorizontally = true;
        game.scale.pageAlignVertically = true;
        game.scale.scaleMode = Phaser.ScaleManager.SHOW_ALL;
        game.renderer.renderSession.roundPixels = true; // no blurring
        this.game.state.start('Preload');
    }
}

var preload = function (game) {
};
preload.prototype = {
    preload: function () {

        var loadingBar = this.add.sprite(game.width / 2, game.height / 2, 'loading');
        loadingBar.anchor.setTo(0.5);
        game.load.setPreloadSprite(loadingBar);
        // load title screen
        game.load.image('title', 'assets/sprites/title-screen.png');
        game.load.image('enter', 'assets/sprites/press-enter-text.png');
        game.load.image('credits', 'assets/sprites/credits-text.png');
        game.load.image('instructions', 'assets/sprites/instructions.png');
        // environment
        game.load.image('background', 'assets/environment/background.png');
        game.load.image('middleground', 'assets/environment/middleground.png');
        //tileset
        game.load.image('tileset', 'assets/environment/tilesets.png');
        game.load.image('walls', 'assets/environment/walls.png');
        game.load.tilemap('map', 'assets/maps/map.json', null, Phaser.Tilemap.TILED_JSON);
        // atlas sprites
        game.load.atlasJSONArray('atlas', 'assets/atlas/atlas.png', 'assets/atlas/atlas.json');
        game.load.atlasJSONArray('atlas-props', 'assets/atlas/atlas-props.png', 'assets/atlas/atlas-props.json');
		//
		game.load.audio('music', ['assets/sound/platformer_level04_loop.ogg']);
    },
    create: function () {
        this.game.state.start('TitleScreen');
    }
}

var titleScreen = function (game) {
};
titleScreen.prototype = {
    create: function () {
        background = game.add.tileSprite(0, 0, gameWidth, gameHeight, 'background');
        middleground = game.add.tileSprite(0, 0, gameWidth, gameHeight, 'middleground');

        this.title = game.add.image(gameWidth / 2, 80, 'title');
        this.title.anchor.setTo(0.5);
        var credits = game.add.image(gameWidth / 2, game.height - 12, 'credits');
        credits.anchor.setTo(0.5);
        this.pressEnter = game.add.image(game.width / 2, game.height - 45, 'enter');
        this.pressEnter.anchor.setTo(0.5);

        var startKey = game.input.keyboard.addKey(Phaser.Keyboard.ENTER);
        startKey.onDown.add(this.startGame, this);

        game.time.events.loop(700, this.blinkText, this);

        this.state = 1;


    },
    blinkText: function () {
        if (this.pressEnter.alpha) {
            this.pressEnter.alpha = 0;
        } else {
            this.pressEnter.alpha = 1;
        }
    },
    update: function () {
        middleground.tilePosition.y -= .2;
    },
    startGame: function () {
        if (this.state == 1) {
            this.state = 2;
            this.title2 = game.add.image(game.width / 2, 40, 'instructions');
            this.title2.anchor.setTo(0.5, 0);
            this.title.destroy();
        } else {
            this.game.state.start('PlayGame');
        }

    }
}

var playGame = function (game) {
};
playGame.prototype = {

    create: function () {
        this.createBackgrounds();
        this.createTilemap();

        this.populate();

        //
        this.createPlayer(4, 7);
        this.decorWorld();
        //
        this.bindKeys();
        //
        game.camera.follow(player, Phaser.Camera.FOLLOW_PLATFORMER);
		// music
        this.music = game.add.audio('music');
        this.music.loop = true;
        this.music.play();
    },

    createBackgrounds: function () {
        background = game.add.tileSprite(0, 0, gameWidth, gameHeight, 'background');
        middleground = game.add.tileSprite(0, 0, gameWidth, gameHeight, 'middleground');
        background.fixedToCamera = true;
        middleground.fixedToCamera = true;

    },

    createTilemap: function () {
        // tile
        globalMap = game.add.tilemap('map');
        globalMap.addTilesetImage('tileset');
        globalMap.addTilesetImage('walls');
        this.layer2 = globalMap.createLayer('Tile Layer 2');
        this.layer2.resizeWorld();
        this.layer = globalMap.createLayer('Tile Layer 1');
        this.layer.resizeWorld();

        // collision
        globalMap.setCollisionBetween(27, 31);
        globalMap.setCollision(33);
        globalMap.setCollisionBetween(182, 185);
        globalMap.setCollisionBetween(182, 185);
        globalMap.setCollision(81);
        globalMap.setCollision(83);
        globalMap.setCollision(85);
        globalMap.setCollision(87);
        globalMap.setCollision(89);
        globalMap.setCollision(114);
        globalMap.setCollision(116);
        globalMap.setCollision(93);
        globalMap.setCollision(170);
        globalMap.setCollisionBetween(172, 173);
        globalMap.setCollision(175);
        globalMap.setCollision(177);
        globalMap.setCollisionBetween(179, 180);
        globalMap.setCollision(166);
        globalMap.setCollision(214);
        globalMap.setCollision(215);
        globalMap.setCollision(238);
        globalMap.setCollision(239);
        globalMap.setCollisionBetween(254, 257);
        globalMap.setCollision(76);
        globalMap.setCollision(100);
        globalMap.setCollision(78);
        globalMap.setCollision(102);
        globalMap.setCollision(248);
        globalMap.setCollision(249);
        globalMap.setCollision(251);
        globalMap.setCollision(252);
        globalMap.setCollision(259);
        globalMap.setCollision(260);
        globalMap.setCollision(119);
        globalMap.setCollision(206);
        globalMap.setCollision(230);
        globalMap.setCollision(209);
        globalMap.setCollision(233);
        // one way
        this.setOneWayCollision(38);
        this.setOneWayCollision(42);
        this.setOneWayCollision(187);
        this.setOneWayCollision(188);

    },

    setOneWayCollision: function (tileIndex) {

        var x, y, tile;
        for (x = 0; x < globalMap.width; x++) {
            for (y = 1; y < globalMap.height; y++) {
                tile = globalMap.getTile(x, y);
                if (tile !== null) {
                    if (tile.index == tileIndex) {
                        tile.setCollision(false, false, true, false);
                    }

                }
            }
        }
    },

    decorWorld: function () {
        this.addProp(0, 6, 'gate-01');

        this.addProp(6, 8, 'plant-big');

        this.addProp(19, 20, 'plant-small');
        this.addProp(13, 19, 'plant-big');
        this.addProp(6, 8, 'plant-big');

        this.addProp(21, 20, 'stone');
        this.addProp(54, 8, 'stone-head');

        this.addProp(52, 1, 'stalactite');
        this.addProp(22, 14, 'stalactite');

    },

    addProp: function (x, y, item) {
        game.add.image(x * 16, y * 16, 'atlas-props', item);
    },

    populate: function () {
        // groups
        projectiles = game.add.group();
        projectiles.enableBody = true;
        //
        enemies = game.add.group();
        enemies.enableBody = true;
        //
        items = game.add.group();
        items.enableBody = true;

        // place enemies
        this.addCrab(56, 6);
        this.addCrab(12, 18);

        this.addJumper(30, 5);
        this.addJumper(48, 5);

        this.addOctopus(52, 17);
        this.addOctopus(23, 17);

        //place items
        this.addItem(30, 3);
        this.addItem(60, 18);
        this.addItem(59, 4);
        this.addItem(4, 17);
    },

    addJumper: function (x, y) {
        var temp = new Jumper(game, x, y);
        game.add.existing(temp);
        enemies.add(temp);
    },

    addOctopus: function (x, y) {
        var temp = new Octopus(game, x, y);
        game.add.existing(temp);
        enemies.add(temp);
    },

    addCrab: function (x, y) {
        var temp = new Crab(game, x, y);
        game.add.existing(temp);
        enemies.add(temp);
    },

    addItem: function (x, y) {
        var temp = new Item(game, x, y);
        game.add.existing(temp);
        items.add(temp);
    },

    createPlayer: function (x, y) {
        x *= 16;
        y *= 16;
        player = game.add.sprite(x, y, 'atlas', 'player-idle-1');
        player.anchor.setTo(0.5);
        game.physics.arcade.enable(player);
        player.body.gravity.y = 500;
        player.body.setSize(11, 40, 35, 24);
        //animations
        var s = 10;
        player.animations.add('idle', Phaser.Animation.generateFrameNames('player-idle-', 1, 4, '', 0), s - 4, true);
        player.animations.add('run', Phaser.Animation.generateFrameNames('player-run-', 1, 10, '', 0), s, true);
        player.animations.add('run-shot', Phaser.Animation.generateFrameNames('player-run-shot-', 1, 10, '', 0), s, true);
        player.animations.add('duck', ['player-duck'], s, true);
        player.animations.add('jump', Phaser.Animation.generateFrameNames('player-jump-', 1, 6, '', 0), s, true);
        player.animations.add('fall', Phaser.Animation.generateFrameNames('player-jump-', 3, 6, '', 0), s, true);
        player.animations.add('shooting', Phaser.Animation.generateFrameNames('player-stand-', 1, 3, '', 0), s + 5, true);
        player.animations.add('hurt', Phaser.Animation.generateFrameNames('player-hurt-', 1, 2, '', 0), 4, false);
        //default animation
        player.animations.play('idle');
    },

    bindKeys: function () {
        this.wasd = {
             jump: game.input.keyboard.addKey(Phaser.Keyboard.C),
             attack: game.input.keyboard.addKey(Phaser.Keyboard.X),
            //jump: game.input.keyboard.addKey(Phaser.Keyboard.Q), // dvorak
            //attack: game.input.keyboard.addKey(Phaser.Keyboard.J), // dvorak
            left: game.input.keyboard.addKey(Phaser.Keyboard.LEFT),
            right: game.input.keyboard.addKey(Phaser.Keyboard.RIGHT),
            down: game.input.keyboard.addKey(Phaser.Keyboard.DOWN)
        }
        game.input.keyboard.addKeyCapture(
            [Phaser.Keyboard.LEFT,
                Phaser.Keyboard.RIGHT,
                Phaser.Keyboard.DOWN
            ]
        );
    },

    update: function () {
        game.physics.arcade.collide(player, this.layer);
        game.physics.arcade.collide(enemies, this.layer);
        game.physics.arcade.collide(projectiles, this.layer, this.hitWall, null, this);
        //
        game.physics.arcade.overlap(enemies, projectiles, this.shotImpact, null, this);
        game.physics.arcade.overlap(player, enemies, this.hurtPlayer, null, this);
        game.physics.arcade.overlap(player, items, this.pickItem, null, this);

        this.movePlayer();
        this.playerAnimations();
        this.parallaxBackground();
        this.hurtFlagManager();

        // this.debugGame();

    },

    pickItem: function (p, i) {
        i.kill();
    },

    hurtPlayer: function (p, enemy) {
        if (hurtFlag) {
            return;
        }
        hurtFlag = true;
        p.animations.play('hurt');
        p.body.velocity.y = -150;
        p.body.velocity.x = (p.scale.x == 1) ? -100 : 100;
    },

    hurtFlagManager: function () {
        // reset hurt when touching ground
        if (hurtFlag && player.body.onFloor()) {
            hurtFlag = false;
        }
    },

    hitWall: function (shot, wall) {
        shot.kill();
        var impact = new ShotImpact(game, shot.x, shot.y);
        game.add.existing(impact);
    },

    shotImpact: function (enemy, shot) {
        var impact = new ShotImpact(game, shot.x, shot.y);
        game.add.existing(impact);

        shot.kill();
        // enemy.kill();
        enemy.health--;
    },

    parallaxBackground: function () {
        middleground.tilePosition.x = this.layer.x * -0.5;
    },

    movePlayer: function () {
        var speed = 100;
        if (this.wasd.left.isDown) {
            player.body.velocity.x = -speed;
            player.scale.x = -1;
        } else if (this.wasd.right.isDown) {
            player.body.velocity.x = speed;
            player.scale.x = 1;
        } else {
            player.body.velocity.x = 0;
        }

        //jump
        if (this.wasd.jump.isDown && player.body.onFloor()) {
            player.body.velocity.y = -200;
        }

        // shooting
        if (this.wasd.attack.isDown) {
            shootingFlag = true;
            this.shoot();
        } else if (this.wasd.attack.onUp) {
            shootingFlag = false;
        }

    },

    playerAnimations: function () {

        if (hurtFlag) {
            return;
        }

        if (player.body.onFloor()) {
            if (player.body.velocity.x != 0) {
                if (shootingFlag) {
                    player.animations.play('run-shot');
                } else {
                    player.animations.play('run');
                }
            } else {
                if (this.wasd.down.isDown) {
                    player.animations.play('duck');
                    isDuck = true;
                } else {
                    isDuck = false;
                    if (shootingFlag) {
                        player.animations.play('shooting');
                    } else {
                        player.animations.play('idle');
                    }
                }
            }

        } else {
            if (player.body.velocity.y > 0) {
                player.animations.play('fall');
            } else if (player.body.velocity.y < 0) {
                player.animations.play('jump');
            }
        }
    },

    shoot: function () {

        if (nextShot > game.time.now) {
            return;
        }

        nextShot = game.time.now + 200; // wait at least half second

        var shot = new Shot(game, player.x, player.y, player.scale.x);
        game.add.existing(shot);
        projectiles.add(shot);
    },

    debugGame: function () {
        //game.debug.spriteInfo(this.player, 30, 30);

        //game.debug.body(player);

        enemies.forEachAlive(this.renderGroup, this);
        items.forEachAlive(this.renderGroup, this);

    },

    renderGroup: function (member) {
        game.debug.body(member);
    },

}

// shot

Shot = function (game, x, y, dir) {
    y = (isDuck) ? y + 8 : y - 5;
    x += (dir == 1) ? 11 : -20;

    Phaser.Sprite.call(this, game, x, y, 'atlas', 'shot-1');
    this.animations.add('shot', Phaser.Animation.generateFrameNames('shot-', 1, 2, '', 0), 10, true);
    this.animations.play('shot');
    game.physics.arcade.enable(this);
    this.body.velocity.x = 220 * dir;

    this.checkWorldBounds = true;

}
Shot.prototype = Object.create(Phaser.Sprite.prototype);
Shot.prototype.constructor = Shot;

Shot.prototype.update = function () {
    if (!this.inWorld) {
        this.destroy();
    }

}

// Crab

Crab = function (game, x, y) {
    x *= 16;
    y *= 16;
    this.health = 5;
    Phaser.Sprite.call(this, game, x, y, 'atlas', 'crab-idle-1');
    this.animations.add('idle', Phaser.Animation.generateFrameNames('crab-idle-', 1, 4, '', 0), 10, true);
    this.animations.add('walk', Phaser.Animation.generateFrameNames('crab-walk-', 1, 4, '', 0), 10, true);
    this.animations.play('idle');
    this.anchor.setTo(0.5);
    game.physics.arcade.enable(this);
    this.body.setSize(16, 25, 16, 7);
    this.body.gravity.y = 500;
    this.body.velocity.x = 60 * game.rnd.pick([1, 0]);
    ;
    this.body.bounce.x = 1;

}
Crab.prototype = Object.create(Phaser.Sprite.prototype);
Crab.prototype.constructor = Crab;

Crab.prototype.update = function () {

    if (this.body.velocity.x < 0) {
        this.scale.x = 1;
    } else {
        this.scale.x = -1;
    }

    if (this.body.velocity.x != 0) {
        this.animations.play('walk');
    } else {
        this.animations.play('idle');
    }

    if (this.health <= 0) {
        var death = new EnemyDeath(game, this.x, this.y);
        game.add.existing(death);
        this.destroy();
    }

}

// Jumper

Jumper = function (game, x, y) {
    x *= 16;
    y *= 16;
    this.health = 5;
    Phaser.Sprite.call(this, game, x, y, 'atlas', 'jumper-idle-1');
    this.animations.add('idle', Phaser.Animation.generateFrameNames('jumper-idle-', 1, 4, '', 0), 7, true);
    this.animations.add('jump', ['jumper-jump'], 10, true);
    this.animations.play('idle');
    this.anchor.setTo(0.5);
    game.physics.arcade.enable(this);
    this.body.setSize(16, 25, 16, 8);
    this.body.gravity.y = 500;
    this.body.bounce.x = 1;
    game.time.events.loop(2000, this.jumperJump, this);

    this.dir = -1;

}
Jumper.prototype = Object.create(Phaser.Sprite.prototype);
Jumper.prototype.constructor = Jumper;

Jumper.prototype.update = function () {

    if (this.body.onFloor()) {
        this.body.velocity.x = 0;
        this.animations.play('idle');
    } else {
        this.body.velocity.x = this.dir * 60;
        this.animations.play('jump');
    }

    if (this.health <= 0) {
        var death = new EnemyDeath(game, this.x, this.y);
        game.add.existing(death);

        this.destroy();

    }

}

Jumper.prototype.jumperJump = function () {
    if (!this.alive) {
        return;
    }

    this.dir *= -1;
    this.body.velocity.y = -200;

}

// octopus

Octopus = function (game, x, y) {
    x *= 16;
    y *= 16;
    this.health = 5;
    Phaser.Sprite.call(this, game, x, y, 'atlas', 'octopus-1');
    game.physics.arcade.enable(this);
    this.anchor.setTo(0.5);
    this.body.setSize(14, 22, 8, 6);
    this.animations.add('fly', Phaser.Animation.generateFrameNames('octopus-', 1, 4, '', 0), 15, true);
    this.animations.play('fly');
    var VTween = game.add.tween(this).to({
        y: y + 50
    }, 1000, Phaser.Easing.Linear.None, true, 0, -1);
    VTween.yoyo(true);
};

Octopus.prototype = Object.create(Phaser.Sprite.prototype);
Octopus.prototype.constructor = Octopus;

Octopus.prototype.update = function () {
    if (this.x > player.x) {
        this.scale.x = 1;
    } else {
        this.scale.x = -1;
    }

    if (this.health <= 0) {
        var death = new EnemyDeath(game, this.x, this.y);
        game.add.existing(death);
        this.destroy();
    }
};

// enemy death

EnemyDeath = function (game, x, y) {
    Phaser.Sprite.call(this, game, x, y, 'atlas', 'enemy-death-1');
    this.anchor.setTo(0.5);
    var anim = this.animations.add('death', Phaser.Animation.generateFrameNames('enemy-death-', 1, 5, '', 0), 18, false);
    this.animations.play('death');
    anim.onComplete.add(function () {
        this.kill();
    }, this);
};

EnemyDeath.prototype = Object.create(Phaser.Sprite.prototype);
EnemyDeath.prototype.constructor = EnemyDeath;

// shot impact

ShotImpact = function (game, x, y) {
    Phaser.Sprite.call(this, game, x, y, 'atlas', 'impact-1');
    this.anchor.setTo(0.5);
    var anim = this.animations.add('impact', Phaser.Animation.generateFrameNames('impact-', 1, 5, '', 0), 18, false);
    this.animations.play('impact');
    anim.onComplete.add(function () {
        this.kill();
    }, this);
};

ShotImpact.prototype = Object.create(Phaser.Sprite.prototype);
ShotImpact.prototype.constructor = ShotImpact;

// item

Item = function (game, x, y) {
    x *= 16;
    y *= 16;
    Phaser.Sprite.call(this, game, x, y, 'atlas', 'power-up-1');
    this.anchor.setTo(0.5);
    this.animations.add('item', Phaser.Animation.generateFrameNames('power-up-', 1, 7, '', 0), 20, true);
    this.animations.play('item');
};

Item.prototype = Object.create(Phaser.Sprite.prototype);
Item.prototype.constructor = Item;









