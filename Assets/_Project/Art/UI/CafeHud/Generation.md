## 2026-11-04 토스트 마침표 보정

Generated with OpenAI built-in ImageGen (기존 원본) + Python 3 / Pillow (후처리). Style reference: ToastNoCustomer.png 및 기존 Toast*.png.

Prompt set: 기존 문구와 도트 글자체를 보존하고 문장 끝 마침표를 정확히 하나로 수정한다.

- ToastCupFirst.png: 정확한 문구 “커피잔을 먼저 올려 주세요.”. 두 번째 마침표 획만 제거.
- ToastNeedTowel.png: 정확한 문구 “행주를 들고 와 주세요.”. 두 번째 마침표 획만 제거.

두 파일 모두 기존 획 마스크를 #380F00 단색 / #FCF0CA 3px 외곽선으로 정리하고 NEAREST로 높이를 맞췄다. 다른 토스트와 동일하게 외곽선 포함 높이 56px(글자 획 50px), PNG 높이 72px, 사방 8px 투명 여백. 알파 0/255. 문구·공백·마침표 하나를 눈으로 확인했다. .meta는 수정하지 않았다.

---

Generated with OpenAI built-in image_gen (?? ??? ?? ??). Style reference: Assets/_Project/Art/UI/OpenPrep/TopBar.png, CardFrame.png, LabelMoney.png; Assets/_Project/Art/UI/Quit/Yes.png, No.png, Panel.png, Message.png; Assets/_Project/Art/UI/Settlement/Star.png.

Prompt set: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding.

??: SFR-013, SFR-021, SFR-025, SFR-057. ?? ???? ?? ??? ????. ? ADR ??? ??.

- StatusBar.png: Dark walnut plank same family as OpenPrep/TopBar.png, shorter ratio 19:3, empty center for runtime HUD. No icons or text.
  - ???: ?? ??? ??? ?? ??? ???? ?? ??? ??. ?? ?? ???. 
- LabelRent.png: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: stylized-concept. Text-only LABEL LIGHT, exact Korean "???" (characters ? / ? / ?). Bold square pixel Hangul like the viewed Quit/Message.png; cream #FCF0CA fill, dark coffee #380F00 outline 3px. Korean glyph body exactly 56px high, uniform 8px transparent padding tightly cropped, one line, no panel, no icon, no decoration. Actual transparent RGBA background. Only one sprite.
  - ???: ??? ?? ??? ???? ?? 56px? ??? ??? ? ??? ???. ?? ??(??? ??) ?? 8px ?? ??. ???????????? ??? ?? ??.
- LabelReputation.png: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Reference the viewed Quit/Message.png typography and OpenPrep/LabelMoney.png light text. One text-only transparent LABEL LIGHT sprite with exact Korean "??". Spell precisely ? / ?. Bold blocky pixel Hangul, cream #FCF0CA fill and dark coffee #380F00 3px outline. No icon, background, button, panel or ornament. Single line. Korean glyph height 56px with tight even 8px transparent padding. Actual transparent background.
  - ???: ??? ?? ??? ???? ?? 56px? ??? ??? ? ??? ???. ?? ??(??? ??) ?? 8px ?? ??. ???????????? ??? ?? ??.
- StockBar.png: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Reference the previously viewed OpenPrep/TopBar.png, CardFrame.png, Quit/Panel.png. Generate ONE sprite StockBar.png, target canvas 600x240 px. Small dark walnut plank, empty center for two ingredient icons and amounts. Match TopBar.png walnut and gold edging. Uniform 8px transparent margin all sides, centered, orthographic front view, dark coffee outline #380F00, cream #FCF0CA, walnut #90471B, honey gold #C57D1F. No extra objects, no background, no text.
  - ???: ?? ??? ??? ?? ??? ???? ?? ??? ??. ?? ?? ???. 
- Ticket.png: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Reference the previously viewed OpenPrep/TopBar.png, CardFrame.png, Quit/Panel.png. Generate ONE sprite Ticket.png, target canvas 400x300 px. Cream paper order ticket with faint receipt lines and small wooden clip centered at top. Empty, no text. Fully contained clip, paper and clip centered, transparent padding. Uniform 8px transparent margin all sides, centered, orthographic front view, dark coffee outline #380F00, cream #FCF0CA, walnut #90471B, honey gold #C57D1F. No extra objects, no background, no text.
  - ???: ?? ??? ??? ?? ??? ???? ?? ??? ??. ?? ?? ???. 
- TicketSelected.png: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Edit target: attached Ticket sprite. Exactly same paper and clip geometry and position as Ticket.png, add glowing honey-amber selected border only. Empty, no text. Preserve every paper edge, receipt line, clip, size, shape and position exactly. Only add a contained narrow honey-amber #DF7015 and #F1C97D luminous stepped-pixel border around the paper without extending its silhouette or moving anything. Transparent background, no text.
  - ???: ?? ??? ??? ?? ??? ???? ?? ??? ??. ?? ?? ???. 
- HandSlotLeft.png: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. ONE square game inventory slot, walnut wooden frame with stepped pixel outline, gold inner edging, the whole center made of solid cream PAPER with faint cream pixel-fleck paper texture and a few very faint horizontal paper fibers, all opaque #FAEAC2. Tiny white left-hand glove at bottom-left INSIDE cream paper area. No empty center, no hole, no vignette, no gray. Draw physical cream paper completely filling entire inner rectangle! Frame and paper are a single solid square object. Background outside object only transparent. Orthographic 2D sprite like previously viewed OpenPrep/CardFrame.png. Canvas 300x300 with even 8px margin. No text.
  - ???: ?? ??? ??? ?? ??? ???? ?? ??? ??. ?? ?? ???. 
- HandSlotRight.png: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Edit target attached HandSlotLeft sprite: make the matching RIGHT hand slot. Change only the tiny glove: move it from lower-left to exactly symmetric lower-right location and reflect its anatomy so its thumb points left inward. Remove original left glove and fill its location with matching opaque cream paper texture. Preserve square wooden frame, corner geometry, gold inset, paper surface, canvas size and every edge position. Everything inside panel fully opaque; transparent only outside. No text. Target 300x300.
  - ???: ?? ??? ??? ?? ??? ???? ?? ??? ??. ?? ?? ???. 
- ContextMenu.png: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Reference the viewed Quit/No.png for button bevel and Quit/Panel.png for walnut frame. ONE sprite ContextMenu.png, target canvas 440x180 px, preserve wide 2.4444444444444446:1 proportions. Small empty cream popup panel with walnut frame and fine gold inner edge. No text or ornaments. Cream interior must be fully OPAQUE solid paper, tiny cream pixel texture, no transparency inside. Uniform 8px transparent padding outside only. Stepped crisp outlines, no background scene, no protrusions. No text, no icons.
  - ???: ?? ??? ??? ?? ??? ???? ?? ??? ??. ?? ?? ???. 
- DiscardButton.png: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Reference the viewed Quit/No.png for button bevel and Quit/Panel.png for walnut frame. ONE sprite DiscardButton.png, target canvas 380x124 px, preserve wide 3.064516129032258:1 proportions. Small beveled cream button, exact centered bold dark-coffee Korean "???" (? / ? / ?), tiny trash-bin icon at left, walnut bevel, match Quit/No.png. No other lettering or ornaments. Cream interior must be fully OPAQUE solid paper, tiny cream pixel texture, no transparency inside. Uniform 8px transparent padding outside only. Stepped crisp outlines, no background scene, no protrusions. Check each Hangul syllable ? ? ?.
  - ???: ?? ??? ??? ?? ??? ???? ?? ??? ??. ?? ?? ???. 
- Toast.png: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. One sprite Toast.png, canvas target 1520x192 px. Wide rounded walnut ribbon banner with opaque cream inside, empty for labels. No text, no ornaments outside frame. Reference previously viewed OpenPrep/TopBar.png and Quit/Panel.png. Keep center solid cream paper, opaque cream pixel-fleck texture, transparent only outside frame. Uniform 8px transparent padding, very wide short proportions. No icons.
  - ???: ?? ??? ??? ?? ??? ???? ?? ??? ??. ?? ?? ???. 
- ToastNoCustomer.png: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. ONE perfect CLEAN text-only sprite. Exact Korean "??? ??? ????.". Typeface: bold squared geometric block Hangul, pixel steps like Quit/Message.png but completely CLEAN edges. Flat uniform dark coffee #380F00 letters, solid cream #FCF0CA outline of 3px at final glyph size 56px. No texture, no grain, no noise, no white fringes, no speckles, no worn edges, no holes in strokes. Empty spaces outside letters are perfectly transparent alpha zero, including all counters. Single line, exact words and final period. ? ? ? SPACE ? ? ? SPACE ? ? ? ? PERIOD. Smooth solid rectangular strokes with deliberate crisp pixel stairs only. No panel, icon, ornament or drop shadow. Target transparent padding 8px, glyph height56px.
  - ???: ??? ?? ??? ???? ?? 56px? ??? ??? ? ??? ???. ?? ??(??? ??) ?? 8px ?? ??. ???????????? ??? ?? ??.
- ToastHandsFull.png: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. ONE CLEAN text-only typography sprite. Exact Korean "????? ?? ????.". Check characters ? / ? / ? / ? / ? /   / ? / ? /   / ? / ? / ? / ? / .. Bold square geometric pixel Hangul with STRAIGHT rectangular strokes and stepped corners. Solid uniform dark coffee #380F00 fill, cream #FCF0CA outline 3px. Target Hangul height56px, tightly cropped with 8px transparent margin. Single line. No texture, grain, noise, speckle, worn edges, distressed print, ornaments, background, panel or shadow. Only clean two-color block lettering and final period. Use Quit/Message.png as LETTER WEIGHT reference only, do not copy its distressed edges. Transparent RGBA exterior and letter counters.
  - ???: ??? ?? ??? ???? ?? 56px? ??? ??? ? ??? ???. ?? ??(??? ??) ?? 8px ?? ??. ???????????? ??? ?? ??.
- ToastNoStock.png: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. ONE CLEAN text-only typography sprite. Exact Korean "??? ????.". Check characters ? / ? / ? /   / ? / ? / ? / ? / .. Bold square geometric pixel Hangul with STRAIGHT rectangular strokes and stepped corners. Solid uniform dark coffee #380F00 fill, cream #FCF0CA outline 3px. Target Hangul height56px, tightly cropped with 8px transparent margin. Single line. No texture, grain, noise, speckle, worn edges, distressed print, ornaments, background, panel or shadow. Only clean two-color block lettering and final period. Use Quit/Message.png as LETTER WEIGHT reference only, do not copy its distressed edges. Transparent RGBA exterior and letter counters.
  - ???: ??? ?? ??? ???? ?? 56px? ??? ??? ? ??? ???. ?? ??(??? ??) ?? 8px ?? ??. ???????????? ??? ?? ??.
- ToastEntryClosed.png: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. ONE CLEAN text-only typography sprite. Exact Korean "?? ??? ???????.". Spell ? / ? SPACE ? / ? / ? SPACE ? / ? / ? / ? / ? / ? / ? PERIOD. Bold square geometric pixel Hangul with STRAIGHT rectangular strokes, stepped corners, blocky even weight like Quit/Message.png. Flat dark coffee #380F00 fill and uniform cream #FCF0CA 3px outline. Single line, all glyphs 56px high, tightly cropped, even8px transparent padding. No distressed edges, texture, grain, speckles, icons, panel, shadow or background. Clean two-color block lettering on actual alpha transparent background including counters.
  - ???: ??? ?? ??? ???? ?? 56px? ??? ??? ? ??? ???. ?? ??(??? ??) ?? 8px ?? ??. ???????????? ??? ?? ??.
- BannerClosed.png: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Reference viewed OpenPrep/TopBar.png, Quit/Panel.png and Settlement/Star.png for style. ONE sprite BannerClosed.png, target 1800x440 px, very wide landscape proportions. Big celebratory walnut sign with exact Korean "?? ??" (? / ? SPACE ? / ?) painted in large cream bold pixel lettering, small coffee cup ornaments inside the left and right ends. Gold inset line, no protrusions, no other text. All details contained within canvas with equal 8px transparent margins. Opaque material inside, transparent only outside silhouette, fine pixel grain, 2-3 blocky shades, no gradients or vignette. Centered Korean painted text ?? ?? must read exactly ? ? SPACE ? ?, no other words.
  - ???: ?? ??? ??? ?? ??? ???? ?? ??? ??. ?? ?? ???. 
- SpeechPanel.png: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Reference viewed OpenPrep/TopBar.png, Quit/Panel.png and Settlement/Star.png for style. ONE sprite SpeechPanel.png, target 2400x400 px, very wide landscape proportions. Wide empty customer dialogue box, opaque cream speech panel, walnut frame, gold inner line, small speech tail at bottom-left contained within canvas. No text or ornament. All details contained within canvas with equal 8px transparent margins. Opaque material inside, transparent only outside silhouette, fine pixel grain, 2-3 blocky shades, no gradients or vignette. Entire cream area is solid opaque lightly textured paper. No words, no icons.
  - ???: ?? ??? ??? ?? ??? ???? ?? ??? ??. ?? ?? ???. 
- PromptUse.png: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. One world-space interaction tag sprite, final 192x72 px exactly. Small cream tag with dark coffee outline; left square keycap "E", right exact Korean "??" (? / ?). Bold pixel lettering, readable at native 192x72. No other text or ornament. Reference Quit/No.png bevel, coffee #380F00, cream #FCF0CA, very simple 2-3 flat shades. WIDE rectangular cream tag with stepped corners, keycap E must be separate small square on left, ?? at right. Keep letters correct ? / ?. No icon. Solid opaque cream tag with lightly textured paper, transparent only outside, equal 4px transparent margin. Proportion 8:3.
  - ???: ?? ??? ??? ?? ??? ???? ?? ??? ??. ?? ?? ???. 
- PromptOpen.png: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Background extraction edit of attached E ?? interaction tag. Keep the whole cream tag, its exact Korean text ?? and E keycap unchanged. Remove ALL dark brown background outside tag including brown halo and vignette. Give tag a thin dark coffee #380F00 stepped outline and equal TRANSPARENT padding around it. The entire exterior must have alpha zero. Preserve opaque cream inside. No shadow or glow. No added objects. Single isolated transparent sprite.
  - ???: ?? ??? ??? ?? ??? ???? ?? ??? ??. ?? ?? ???. 
- GaugeFrame.png: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. ONE world-space sprite GaugeFrame.png target EXACTLY 128x24 px. Thin horizontal walnut gauge frame with empty opaque dark coffee interior, rectangular 128x24. 120x16 opening including thin frame, simple crisp pixel edges, no text or decorations. Reference viewed TopBar.png walnut palette but MUCH SIMPLER, 4px wooden thickness, flat black-coffee empty opaque center, no grain needed, straight horizontal thin rectangle, even 1px external transparent padding. No icons or labels.
  - ???: ?? ??? ??? ?? ??? ???? ?? ??? ??. ?? ?? ???. 
- GaugeFill.png: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. ONE world-space sprite GaugeFill.png target EXACTLY 120x16 px. Plain WHITE rectangular fill bar for a game gauge, exactly 120x16, subtle 1-2px lighter white top highlight, near-white body #F7F7F7. No border, no wood, no text, no rounding, no shadow. Flat grayscale only, tintable. Very long slim horizontal 7.5:1 RECTANGLE, sharp square corners, fill entire sprite including edges, grayscale only, no wood or brown or cream. No exterior shadow, no background. Opaque white object; transparency outside only if any. Designed to fit inside GaugeFrame, tintable solid white.
  - ???: ?? ??? ??? ?? ??? ???? ?? ??? ??. ?? ?? ???. 

??: RGBA PNG, ?? 0/255, ?? ?? ??. ?? ??? ?? ??? ???.

Ticket/TicketSelected? ??? ?? ??(8, 8)?(392, 292)? ??? ??? ???. HandSlotLeft/HandSlotRight? ?? ?? ??? ????. GaugeFill? ?? ???? ??? ???? ??? ??? 120?16px ??? ???? ?? ?? ??? ??.

SpeechPanel? ??? 2400?400px??? Unity ??? Max Size? 4096?? ???? ?? ??? ????. ?? ??? ART_STYLE.md ?? Max Size 2048? ????.

| ?? | ?? ?? | ?? ?? | ?? ?? ?? |
|---|---|---|---|
| StatusBar.png | 1520?240 | ? | ? |
| LabelRent.png | 178?72 | ? | 56px |
| LabelReputation.png | 132?72 | ? | 56px |
| StockBar.png | 600?240 | ? | ? |
| Ticket.png | 400?300 | ? | ? |
| TicketSelected.png | 400?300 | ? | ? |
| HandSlotLeft.png | 300?300 | ? | ? |
| HandSlotRight.png | 300?300 | ? | ? |
| ContextMenu.png | 440?180 | ? | ? |
| DiscardButton.png | 380?124 | ? | ? |
| Toast.png | 1520?192 | ? | ? |
| ToastNoCustomer.png | 504?72 | ? | 56px |
| ToastHandsFull.png | 568?72 | ? | 56px |
| ToastNoStock.png | 386?72 | ? | 56px |
| ToastEntryClosed.png | 550?72 | ? | 56px |
| BannerClosed.png | 1800?440 | ? | ? |
| SpeechPanel.png | 2400?400 | ? | ? |
| PromptUse.png | 192?72 | ? | ? |
| PromptOpen.png | 192?72 | ? | ? |
| GaugeFrame.png | 128?24 | ? | ? |
| GaugeFill.png | 120?16 | ??? | ? |


## 경고 토스트 LABEL 추가 (2026-11-04)

Generated with OpenAI built-in image_gen. Style reference: Assets/_Project/Art/UI/CafeHud/ToastNoCustomer.png, Assets/_Project/Art/UI/Quit/Message.png.

Prompt set: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding.

- 공통 규격: 기본색 다크 커피 #380F00 + 크림 #FCF0CA 외곽선, 한 줄, 장식 없는 투명 RGBA. 참조 ToastNoCustomer.png와 동일하게 외곽선 포함 글자 높이 56px, 사방 투명 여백 8px. 글자 높이는 알파가 0보다 큰 영역의 세로 경계로 측정했다.
- 제작 후처리: 생성 이미지의 두 색 팔레트 및 알파 경계를 정리하고 최근접 보간으로 크기를 맞췄다. 원본 참조 PNG는 수정하지 않았다. 한글과 띄어쓰기·마침표는 생성본 및 최종 저장본을 눈으로 확인했다.

- ToastNoCleanCup.png: 정확한 문구 "깨끗한 커피잔이 없습니다.".
  - Prompt: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. ONE text-only transparent Korean LABEL sprite, same typography as reference ToastNoCustomer.png. Exact text: "깨끗한 커피잔이 없습니다." Check syllables 깨 / 끗 / 한 SPACE 커 / 피 / 잔 / 이 SPACE 없 / 습 / 니 / 다 / period. Bold squared geometric pixel Hangul, dark coffee #380F00 flat fill and cream #FCF0CA 3px outline, clean straight rectangular strokes and crisp stepped corners. One single horizontal line, consistent Hangul glyph height 56px, tightly crop with even 8px transparent padding. No panel, backdrop, ornaments, shadow, blur, grain or noise. Generate only this asset.
- ToastSetPortafilter.png: 정확한 문구 "포터필터를 먼저 세팅해 주세요.".
  - Prompt: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. ONE CLEAN text-only transparent LABEL. Verbatim Korean text "포터필터를 먼저 세팅해 주세요.". Syllables: 포 / 터 / 필 / 터 / 를 /   / 먼 / 저 /   / 세 / 팅 / 해 /   / 주 / 세 / 요 / .. Match ToastNoCustomer.png bold square geometric pixel Hangul typography, dark coffee #380F00 fill, cream #FCF0CA 3px outline. Uniform 56px glyph height including outline, single line, tight 8px transparent margin. Only clean solid two-color letters, spaces and final period. No panel, decorations, shadow, noise, texture or blur. Check every Korean character.
- ToastNoBean.png: 정확한 문구 "원두가 부족합니다.".
  - Prompt: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. ONE CLEAN text-only transparent LABEL. Verbatim Korean text "원두가 부족합니다.". Syllables: 원 / 두 / 가 /   / 부 / 족 / 합 / 니 / 다 / .. Match ToastNoCustomer.png bold square geometric pixel Hangul typography, dark coffee #380F00 fill, cream #FCF0CA 3px outline. Uniform 56px glyph height including outline, single line, tight 8px transparent margin. Only clean solid two-color letters, spaces and final period. No panel, decorations, shadow, noise, texture or blur. Check every Korean character.
- ToastEmptyPortafilterOnly.png: 정확한 문구 "빈 포터필터만 사용할 수 있습니다.".
  - Prompt: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. ONE CLEAN text-only transparent LABEL. Verbatim Korean text "빈 포터필터만 사용할 수 있습니다.". Syllables: 빈 /   / 포 / 터 / 필 / 터 / 만 /   / 사 / 용 / 할 /   / 수 /   / 있 / 습 / 니 / 다 / .. Match ToastNoCustomer.png bold square geometric pixel Hangul typography, dark coffee #380F00 fill, cream #FCF0CA 3px outline. Uniform 56px glyph height including outline, single line, tight 8px transparent margin. Only clean solid two-color letters, spaces and final period. No panel, decorations, shadow, noise, texture or blur. Check every Korean character.
- ToastTampedOnly.png: 정확한 문구 "탬핑을 완료한 포터필터만 장착할 수 있습니다.".
  - Prompt: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. ONE clean text-only transparent LABEL. Verbatim text "탬핑을 완료한 포터필터만 장착할 수 있습니다.". Exact syllables 탬 / 핑 / 을 /   / 완 / 료 / 한 /   / 포 / 터 / 필 / 터 / 만 /   / 장 / 착 / 할 /   / 수 /   / 있 / 습 / 니 / 다 / .. Reference ToastNoCustomer.png for bold square pixel Hangul. Dark coffee #380F00 fill, cream #FCF0CA outline 3px. Uniform glyph height56px including outline, one single line, 8px transparent padding. Precise flat colors, mathematically clean edges, no distress, no glow, no grain, no speckles, no shadow, no background, no decorations. Only text, spaces, final period.
- ToastAttachPortafilter.png: 정확한 문구 "포터필터를 장착해 주세요.".
  - Prompt: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. ONE clean text-only transparent LABEL. Verbatim text "포터필터를 장착해 주세요.". Exact syllables 포 / 터 / 필 / 터 / 를 /   / 장 / 착 / 해 /   / 주 / 세 / 요 / .. Reference ToastNoCustomer.png for bold square pixel Hangul. Dark coffee #380F00 fill, cream #FCF0CA outline 3px. Uniform glyph height56px including outline, one single line, 8px transparent padding. Precise flat colors, mathematically clean edges, no distress, no glow, no grain, no speckles, no shadow, no background, no decorations. Only text, spaces, final period.
- ToastPlaceShotGlass.png: 정확한 문구 "빈 컵을 놓아 주세요.".
  - Prompt: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. ONE clean text-only transparent LABEL. Verbatim text "빈 컵을 놓아 주세요.". Exact syllables 빈 /   / 컵 / 을 /   / 놓 / 아 /   / 주 / 세 / 요 / .. Reference ToastNoCustomer.png for bold square pixel Hangul. Dark coffee #380F00 fill, cream #FCF0CA outline 3px. Uniform glyph height56px including outline, one single line, 8px transparent padding. Precise flat colors, mathematically clean edges, no distress, no glow, no grain, no speckles, no shadow, no background, no decorations. Only text, spaces, final period.
- ToastShotAlready.png: 정확한 문구 "이미 샷이 담긴 컵입니다.".
  - Prompt: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. ONE clean transparent text-only sprite. Exact Korean "이미 샷이 담긴 컵입니다.". Syllables 이 / 미 /   / 샷 / 이 /   / 담 / 긴 /   / 컵 / 입 / 니 / 다 / .. Match reference bold square pixel Hangul, dark coffee #380F00 fill and cream #FCF0CA 3px outline. Single line, consistent 56px glyphs including outline, 8px transparent margin. No background, panel, ornament, shadow, blur, glow, distress or speckle. Clean solid strokes, exact spaces and punctuation.
- ToastNoIce.png: 정확한 문구 "얼음이 부족합니다.".
  - Prompt: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. ONE clean transparent text-only sprite. Exact Korean "얼음이 부족합니다.". Syllables 얼 / 음 / 이 /   / 부 / 족 / 합 / 니 / 다 / .. Match reference bold square pixel Hangul, dark coffee #380F00 fill and cream #FCF0CA 3px outline. Single line, consistent 56px glyphs including outline, 8px transparent margin. No background, panel, ornament, shadow, blur, glow, distress or speckle. Clean solid strokes, exact spaces and punctuation.
- ToastPlaceCup.png: 정확한 문구 "커피잔을 놓아 주세요.".
  - Prompt: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. ONE clean transparent text-only sprite. Exact Korean "커피잔을 놓아 주세요.". Syllables 커 / 피 / 잔 / 을 /   / 놓 / 아 /   / 주 / 세 / 요 / .. Match reference bold square pixel Hangul, dark coffee #380F00 fill and cream #FCF0CA 3px outline. Single line, consistent 56px glyphs including outline, 8px transparent margin. No background, panel, ornament, shadow, blur, glow, distress or speckle. Clean solid strokes, exact spaces and punctuation.
- ToastCupFirst.png: 정확한 문구 "커피잔을 먼저 올려 주세요.".
  - Prompt: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. ONE clean transparent text-only sprite. Exact Korean "커피잔을 먼저 올려 주세요.". Syllables 커 / 피 / 잔 / 을 /   / 먼 / 저 /   / 올 / 려 /   / 주 / 세 / 요 / .. Match reference bold square pixel Hangul, dark coffee #380F00 fill and cream #FCF0CA 3px outline. Single line, consistent 56px glyphs including outline, 8px transparent margin. No background, panel, ornament, shadow, blur, glow, distress or speckle. Clean solid strokes, exact spaces and punctuation.
  - 재생성: 최초 출력에 배경이 섞여 제외했다. Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. ONE isolated text-only transparent LABEL. Exact Korean "커피잔을 먼저 올려 주세요.". Glyphs 커 / 피 / 잔 / 을 SPACE 먼 / 저 SPACE 올 / 려 SPACE 주 / 세 / 요 / .. Bold squared pixel Hangul, flat #380F00 fill, #FCF0CA outline3px. Match ToastNoCustomer. Only letters and spaces, alpha-zero exterior and counters; no background rectangle, no cream plate, no shadow or glow. Single line,56px glyphs including outline,8px transparent margin.
- ToastNoIngredient.png: 정확한 문구 "재료가 부족합니다.".
  - Prompt: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. ONE isolated text-only transparent PNG label, exact "재료가 부족합니다.". Syllables 재 / 료 / 가 /   / 부 / 족 / 합 / 니 / 다 / .. Bold pixel Hangul exactly like ToastNoCustomer reference. Flat dark coffee #380F00 with cream #FCF0CA outline 3px, 56px glyph height including outline, single line,8px padding. Genuine alpha transparency outside letters AND letter counters. No panel, rectangle, backdrop, background color, glow, shadow, distress, texture, fringe or speckles. Precise text and punctuation.
- ToastNothingToComplete.png: 정확한 문구 "완성할 음료가 없습니다.".
  - Prompt: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. ONE isolated text-only transparent PNG label, exact "완성할 음료가 없습니다.". Syllables 완 / 성 / 할 /   / 음 / 료 / 가 /   / 없 / 습 / 니 / 다 / .. Bold pixel Hangul exactly like ToastNoCustomer reference. Flat dark coffee #380F00 with cream #FCF0CA outline 3px, 56px glyph height including outline, single line,8px padding. Genuine alpha transparency outside letters AND letter counters. No panel, rectangle, backdrop, background color, glow, shadow, distress, texture, fringe or speckles. Precise text and punctuation.
- ToastBlenderEmpty.png: 정확한 문구 "믹서기에 재료를 넣어 주세요.".
  - Prompt: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. ONE isolated text-only transparent PNG label, exact "믹서기에 재료를 넣어 주세요.". Syllables 믹 / 서 / 기 / 에 /   / 재 / 료 / 를 /   / 넣 / 어 /   / 주 / 세 / 요 / .. Bold pixel Hangul exactly like ToastNoCustomer reference. Flat dark coffee #380F00 with cream #FCF0CA outline 3px, 56px glyph height including outline, single line,8px padding. Genuine alpha transparency outside letters AND letter counters. No panel, rectangle, backdrop, background color, glow, shadow, distress, texture, fringe or speckles. Precise text and punctuation.
- ToastBlendFirst.png: 정확한 문구 "먼저 믹서기를 돌려 주세요.".
  - Prompt: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. ONE isolated text-only transparent PNG label, exact "먼저 믹서기를 돌려 주세요.". Syllables 먼 / 저 /   / 믹 / 서 / 기 / 를 /   / 돌 / 려 /   / 주 / 세 / 요 / .. Bold pixel Hangul exactly like ToastNoCustomer reference. Flat dark coffee #380F00 with cream #FCF0CA outline 3px, 56px glyph height including outline, single line,8px padding. Genuine alpha transparency outside letters AND letter counters. No panel, rectangle, backdrop, background color, glow, shadow, distress, texture, fringe or speckles. Precise text and punctuation.
- ToastNoPlate.png: 정확한 문구 "깨끗한 접시가 없습니다.".
  - Prompt: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. ONE text-only transparent sprite label with exact Korean "깨끗한 접시가 없습니다.". Check characters 깨 / 끗 / 한 /   / 접 / 시 / 가 /   / 없 / 습 / 니 / 다 / .. Match reference bold square geometric pixel Hangul; flat dark coffee #380F00 lettering, cream #FCF0CA 3px outline. Single line, glyph height56px including outline, tight 8px alpha-zero padding. No background, panel, ornament, shadow, distress, glow, grain or speckles. True transparent exterior and counters.
- ToastNeedTowel.png: 정확한 문구 "행주를 들고 와 주세요.".
  - Prompt: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. ONE text-only transparent sprite label with exact Korean "행주를 들고 와 주세요.". Check characters 행 / 주 / 를 /   / 들 / 고 /   / 와 /   / 주 / 세 / 요 / .. Match reference bold square geometric pixel Hangul; flat dark coffee #380F00 lettering, cream #FCF0CA 3px outline. Single line, glyph height56px including outline, tight 8px alpha-zero padding. No background, panel, ornament, shadow, distress, glow, grain or speckles. True transparent exterior and counters.
- ToastReturnScoop.png: 정확한 문구 "스쿱을 먼저 반납해 주세요.".
  - Prompt: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. ONE text-only transparent sprite label with exact Korean "스쿱을 먼저 반납해 주세요.". Check characters 스 / 쿱 / 을 /   / 먼 / 저 /   / 반 / 납 / 해 /   / 주 / 세 / 요 / .. Match reference bold square geometric pixel Hangul; flat dark coffee #380F00 lettering, cream #FCF0CA 3px outline. Single line, glyph height56px including outline, tight 8px alpha-zero padding. No background, panel, ornament, shadow, distress, glow, grain or speckles. True transparent exterior and counters.
- ToastExtracting.png: 정확한 문구 "추출 중에는 뺄 수 없습니다.".
  - Prompt: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. ONE text-only transparent sprite label with exact Korean "추출 중에는 뺄 수 없습니다.". Check characters 추 / 출 /   / 중 / 에 / 는 /   / 뺄 /   / 수 /   / 없 / 습 / 니 / 다 / .. Match reference bold square geometric pixel Hangul; flat dark coffee #380F00 lettering, cream #FCF0CA 3px outline. Single line, glyph height56px including outline, tight 8px alpha-zero padding. No background, panel, ornament, shadow, distress, glow, grain or speckles. True transparent exterior and counters.


## E 키 안내

﻿Generated with Python 3 / Pillow (기존 이미지 픽셀 추출). Style reference: Assets/_Project/Art/UI/CafeHud/PromptUse.png, PromptOpen.png.

Prompt set: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding.

- PromptKey.png: PromptUse.png에서 E 키 외곽선과 내부 픽셀을 그대로 추출했다. 안내 문구와 키 밖 배경 패널은 제거했다. image_gen 재생성과 확대·축소는 하지 않았다.
  - 원본 추출 범위: x=24..65, y=16..56 (0부터 시작, 양 끝 포함). 각 행의 x=20..69 중 R<210인 픽셀의 양 끝을 키 경계로 삼아 그 사이 원본 픽셀만 복사했다.
  - 원본 키: 42×41px. 출력: 50×50px PNG, RGBA. 여백: 왼쪽·오른쪽·위 4px, 아래 5px. 원본 가로·세로가 1px 달라, 원본 크기와 정사각형을 유지하면 사방 정확히 4px를 동시에 만족할 수 없다.
  - 실제 파일 검사 통과: 알파 값 {0,255}, 불투명 영역 bbox=(4,4,46,45). 모든 투명 픽셀은 RGBA=(0,0,0,0). 모든 불투명 픽셀은 원본 대응 좌표와 RGBA까지 동일하다.
  - 저장소 파일 변경 없음. Unity 적용 시 Point (no filter), Mip Maps 끔, Alpha Is Transparency 켬. 기존 키의 표시 크기를 유지하려면 기존 프롬프트와 같은 픽셀당 표시 배율을 사용한다.

## PR #8 교체 (2026-11)

Codex(AI 에이전트)로 다시 만들어 교체한 것으로 보인다. 도구 확인과 파일별 프롬프트는 작성자에게 받아 보충한다.

- 교체한 파일: BannerClosed.png, LabelRent.png, LabelReputation.png, Ticket.png, TicketSelected.png, Toast.png

## 2026-11-09 인벤토리 칸 손 그림 제거

- 도구: Python 3 / Pillow. 이미지 생성 도구는 사용하지 않았다.
- 대상: HandSlotLeft.png, HandSlotRight.png. 각 손 아이콘의 행별 윤곽 안에서 색상으로 손 픽셀을 구분하고, 윤곽 내부의 빈 부분만 포함한 마스크로 제거했다. 손가락 사이의 열린 종이 영역은 유지했다.
- 방법: 원본의 같은 높이에 있는 인접 크림색 종이 픽셀을 그대로 복사했다(왼쪽 파일은 x+44, 오른쪽 파일은 x-44). 리샘플링이나 흐림 없이 기존 종이의 색과 질감을 사용했다.
- 보존 검증: 두 파일 모두 300×300 RGBA, 알파 채널 전체와 마스크 밖 픽셀이 원본과 동일하다. 나무 프레임·금색 선과 .meta는 수정하지 않았다.
- 변경 픽셀: HandSlotLeft.png 661개, x=35..65 / y=233..262; HandSlotRight.png 687개, x=236..266 / y=233..262. 좌표는 왼쪽 위 (0,0) 기준이며 양 끝을 포함한다.
- 수정 전후 비교: %TEMP%/cafe-time-illustration-preview/HandSlot.png.

## 2026-11-09 게이지 캐러멜 캡슐로 교체

- 도구: Python 3 / Pillow로 픽셀을 직접 찍었다. 이미지 생성 도구는 사용하지 않았다. 사용자가 시안 3개(캐러멜 캡슐·라떼 거품·커피콩 끝장식) 중 캐러멜 캡슐을 골랐다.
- `GaugeFrame.png` (128×24, 크기 유지): 양끝 반원 캡슐. 바깥 2px 진한 호두나무 `#5D2608` 외곽선, 안쪽 진한 크림 `#F7DCA5` 트랙, 트랙 위 3줄은 오목한 그림자 `#E6C084`. 알파는 0/255인 계단식 경계.
- `GaugeFill.png` (120×16, 크기 유지): 회색조 캡슐. 기본 `#DBDBDB`, 아래 5줄 그림자 `#B3B3B3`, 위 2줄 하이라이트 `#FFFFFF`(양끝 7px 제외). 코드가 색을 곱한다: 기기 창과 맵 작업 게이지는 캐러멜 `#FFA032`(기본 부분이 `#DB8A2B`로 보임), 손님 인내심은 기존처럼 초록 `#6FB34A` → 빨강 `#C0392B`.
- 9-slice 테두리(좌우만): GaugeFrame 12px, GaugeFill 8px. `CafeSceneBuilder` 가 임포트 시 설정한다. 채움은 너비를 줄여 표시해 양끝 반원이 유지된다.


## 2026-11-12 ToastEmptyCupOnly

- 도구: OpenAI built-in image_gen + Python 3 / Pillow 후처리.
- 스타일 참조: 같은 폴더의 `ToastNoCleanCup.png`, `ToastHandsFull.png`, `ToastCupFirst.png`, `ToastEmptyPortafilterOnly.png`. 기존 PNG의 채움색·알파 마스크·글자 경계를 분석했다. `ToastCupFirst.png`는 획 마스크를 사방 3px 확장한 영역이 외곽선 마스크와 정확히 일치한다.
- 파일: `ToastEmptyCupOnly.png`, 589×72px, RGBA. 정확한 문구: "빈 컵만 되돌릴 수 있습니다."
- 프롬프트: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. ONE text-only transparent Korean toast sprite matching the reference PNG typography. Exact text: "빈 컵만 되돌릴 수 있습니다." Check syllables: 빈 SPACE 컵 만 SPACE 되 돌 릴 SPACE 수 SPACE 있 습 니 다 PERIOD. Bold square block Hangul, flat dark coffee #380F00 fill, cream #FCF0CA 3px outline. Ink height 50px, outline-inclusive height 56px, transparent padding 8px, canvas height 72px. One horizontal line, one final period, no decoration, panel, icon, shadow, glow or texture. Transparent exterior and counters.
- 후처리: 생성본에서 알파 ≥128, R<130, G<80, B<60인 픽셀을 글자 획 마스크로 추출하고 경계로 잘랐다. NEAREST로 획 높이 50px에 맞춘 뒤 #380F00 단색으로 채웠다. Pillow MaxFilter(7)로 사방 3px 확장한 마스크에 #FCF0CA 외곽선을 만들었다. 외곽선 포함 높이 56px, 사방 투명 여백 8px, 알파 {0,255}, 투명 픽셀 RGBA=(0,0,0,0)으로 정리했다.
- 검수: 최종 PNG를 NEAREST 2배 확대하여 `빈 / 공백 / 컵 / 만 / 공백 / 되 / 돌 / 릴 / 공백 / 수 / 공백 / 있 / 습 / 니 / 다 / .`를 글자별로 눈으로 확인했다. 한 줄·공백 4개·마침표 1개이며 장식이 없다. 기존 PNG와 .meta·코드·씬·프리팹·에셋은 변경하지 않았다.

## 2026-11-12 ToastOrderSkipped.png

- 도구: OpenAI built-in image_gen + Python 3 / Pillow 후처리.
- 스타일 참조: 같은 폴더의 `ToastNoCleanCup.png`, `ToastEmptyCupOnly.png`를 분석하고 생성 도구에도 참조 이미지로 제공했다. 두 파일 모두 채움색 `#380F00`, 외곽선 `#FCF0CA`, 전체 높이 72px, 사방 투명 여백 8px이다. `ToastNoCleanCup.png`의 채움 높이는 48px이고 `ToastEmptyCupOnly.png`는 50px이므로, 새 파일은 최신 `ToastEmptyCupOnly.png`의 채움 높이 50px·사방 3px 외곽선 규격을 따른다.
- 파일: `ToastOrderSkipped.png`, 758×72px, RGBA. 정확한 문구: "먼저 들어온 주문부터 서빙해 주세요.".
- 프롬프트: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. ONE text-only transparent Korean toast sprite matching reference typography: very bold squared geometric block Hangul, heavy straight rectangular strokes and stepped corners. Exact text "먼저 들어온 주문부터 서빙해 주세요." Check 먼 / 저 / SPACE / 들 / 어 / 온 / SPACE / 주 / 문 / 부 / 터 / SPACE / 서 / 빙 / 해 / SPACE / 주 / 세 / 요 / PERIOD. One single horizontal line, four spaces, one final period. Dark coffee #380F00 fill, cream #FCF0CA square 3px outline relative to 50px ink height, outline-inclusive height 56px, 8px transparent padding, final canvas height 72px. Proportional character widths and uniform glyph size. Transparent exterior and counters. No panel, backdrop, decorations, icons, shadow, glow, texture or extra words. NEAREST-only normalization.
- 후처리: 생성본에서 알파 ≥128, R<130, G<80, B<60인 픽셀을 채움 마스크로 추출하고 경계로 잘랐다. 종횡비를 유지하며 NEAREST로 채움 높이 50px에 맞췄다. 채움을 `#380F00` 단색으로 정리하고 Pillow MaxFilter(7)로 사방 3px 확장하여 `#FCF0CA` 외곽선을 다시 적용했다. 외곽선 포함 높이 56px, 사방 투명 여백 8px, 알파 {0,255}, 투명 픽셀 RGBA=(0,0,0,0).
- 검수: 최종 PNG의 NEAREST 2배 확대본에서 `먼 / 저 / 공백 / 들 / 어 / 온 / 공백 / 주 / 문 / 부 / 터 / 공백 / 서 / 빙 / 해 / 공백 / 주 / 세 / 요 / .`를 글자별로 눈으로 대조했다. 한 줄, 공백 4개, 마지막 마침표 1개, 장식 없음. 기존 PNG와 .meta·코드·씬·프리팹·에셋은 수정하지 않았다.



## 2026-11-12 ToastWaterFirst.png

- 도구: OpenAI 내장 `image_gen.imagegen` + Python 3 / Pillow 후처리.
- 참조: `ToastNoCleanCup.png`, `ToastOrderSkipped.png`를 직접 확인하고 생성 도구에도 제공했다. 최신 ToastOrderSkipped와 동일하게 채움 높이50px, 크림 외곽선3px, 사방 투명 여백8px, 전체 높이72px로 맞췄다.
- 파일: **ToastWaterFirst.png, 514×72, RGBA**, 정확한 문구 **"물을 먼저 받아 주세요."**. 한 줄, 공백3개, 마지막 마침표1개.
- 프롬프트: “Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. ONE text-only transparent Korean toast matching reference bold square block typography and proportions. Exact text "물을 먼저 받아 주세요." Check syllables 물 / 을 / SPACE / 먼 / 저 / SPACE / 받 / 아 / SPACE / 주 / 세 / 요 / PERIOD. One single horizontal line, three spaces, one final period. Dark coffee #380F00 fill, cream #FCF0CA square3px outline relative to50px ink height, total height72 with8px transparent padding around56px outline-inclusive ink. Heavy straight rectangular strokes and stepped corners. Transparent exterior and counters. No panel, backdrop, decorations, icons, shadow, glow, texture or extra words.”
- 후처리: 생성본에서 알파≥128·R<130·G<80·B<60의 어두운 획만 추출하고 크롭, 종횡비를 유지하며 NEAREST로 높이50px에 맞췄다. #380F00 채움, MaxFilter(7)로 #FCF0CA 사방3px 외곽선을 다시 만들었다. 외곽선 포함56px, 투명 여백8px, 알파{0,255}, 투명 RGB0. 기존 토스트와 같은 정규화 방식이다.
- 검수: 최종 PNG의 NEAREST2배 확대본에서 **물 / 을 / 공백 / 먼 / 저 / 공백 / 받 / 아 / 공백 / 주 / 세 / 요 / .** 를 글자별로 눈으로 확인했다. DrinkTools의 부드러운 그림 예외와 달리 이 토스트는 HUD 도트 글자이며 Unity에서 **Point(no filter)**, Single, Mip Maps끔, Alpha Is Transparency켬으로 임포트한다. 기본 표시는257×36(원본 절반), 기존 토스트 표시 영역에 가운데 정렬한다.
- 생성 원본: `C:/Users/k9425/.codex/generated_images/01a1ceb5-1e0c-7031-92cd-2a8d521c4cfc/exec-7c1f565b-3046-43f5-bb7e-c1ebd2f26bd0.png`.
- 미리보기: Assets 밖 `C:/Users/k9425/AppData/Local/Temp/claude/bd37fc10698b196c/fec95591-d3a3-47b1-89d7-0c4e1896d871/scratchpad/drinktools-phase2/ToastWaterFirst2x.png`. 기존 PNG·.meta·코드·씬·프리팹·에셋은 수정하지 않았다.


## 2026-11-12 Jua 재조판 (ADR-0026)

- 도구: Python / Pillow의 Jua-Regular.ttf 조판(4배 안티앨리어싱, Lanczos 축소). 바탕이 필요한 파일은 주변 재질 표본 복원 또는 NumPy 확산 인페인트를 사용했다. 글리프를 1px 굵게 만들거나 최근접 보간하는 도트 처리는 하지 않았다.
- 스타일 참조: 이 폴더의 작업 직전 PNG, `Assets/_Project/Art/Fonts/Jua-Regular.ttf`, `docs/ART_STYLE.md` 4장, 승인 ADR-0026. 기존 Galmuri11 조판·AI 글자 설명은 이 절로 대체하며, 과거 기록은 삭제하지 않았다.
- 문구·공백·문장부호는 작업 직전 실제 PNG와 생성 기록을 대조했다. 고정 문구만 재조판하고 가변 게임 값은 새로 넣지 않았다. 글자 없는 그림·아이콘·이미 제거된 패널 제목은 그대로 보존했다.
- Jua에 없는 가운뎃점(`·`)은 같은 Jua의 마침표 글리프를 가운데 높이에 배치했다. 다른 폰트의 글리프를 섞지 않았다.
- 투명 라벨의 채움 높이는 S 56 / M 72 / L 96px이며, 원래 캔버스 높이와 최소 8px 투명 여백을 유지했다. 기존 높이 72px인 외곽선 라벨은 채움 50px, 높이 48px인 라벨은 채움 36px·외곽선 2px·상하 여백 4px로 기존 캔버스 높이를 우선했다. `Pos/TabDessert.png`의 높이 76px는 채움 56px·외곽선 3px·상하 여백 7px다. 해당하는 파일만 아래 표의 규격 예외가 적용된다. 폭이 부족한 라벨은 폭만 늘렸다.

| 파일 | 고정 문구 | 채움 높이 | 채움색 | 외곽선 | 처리 방식 | 캔버스 (전 → 후) |
|---|---|---:|---|---|---|---|
| LabelRent.png | 임대료 | 50px | #FCF0CA | #380F00 3px | Pillow Jua 4배 안티앨리어싱 조판 | 178×72 |
| LabelReputation.png | 평판 | 50px | #FCF0CA | #380F00 3px | Pillow Jua 4배 안티앨리어싱 조판 | 132×72 |
| ToastAttachPortafilter.png | 포터필터를 장착해 주세요. | 50px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 573×72 → **590×72 (폭 확대)** |
| ToastBlenderEmpty.png | 믹서기에 재료를 넣어 주세요. | 50px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 551×72 → **658×72 (폭 확대)** |
| ToastBlendFirst.png | 먼저 믹서기를 돌려 주세요. | 50px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 546×72 → **608×72 (폭 확대)** |
| ToastCupFirst.png | 커피잔을 먼저 올려 주세요. | 50px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 540×72 → **606×72 (폭 확대)** |
| ToastEmptyCupOnly.png | 빈 컵만 되돌릴 수 있습니다. | 50px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 589×72 → **615×72 (폭 확대)** |
| ToastEmptyPortafilterOnly.png | 빈 포터필터만 사용할 수 있습니다. | 50px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 675×72 → **750×72 (폭 확대)** |
| ToastEntryClosed.png | 손님 입장이 마감되었습니다. | 50px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 550×72 → **629×72 (폭 확대)** |
| ToastExtracting.png | 추출 중에는 뺄 수 없습니다. | 50px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 548×72 → **596×72 (폭 확대)** |
| ToastHandsFull.png | 인벤토리가 가득 찼습니다. | 50px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 568×72 → **587×72 (폭 확대)** |
| ToastNeedTowel.png | 행주를 들고 와 주세요. | 50px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 484×72 → **516×72 (폭 확대)** |
| ToastNoBean.png | 원두가 부족합니다. | 50px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 449×72 |
| ToastNoCleanCup.png | 깨끗한 커피잔이 없습니다. | 50px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 555×72 → **580×72 (폭 확대)** |
| ToastNoCustomer.png | 주문할 손님이 없습니다. | 50px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 504×72 → **526×72 (폭 확대)** |
| ToastNoIce.png | 얼음이 부족합니다. | 50px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 451×72 |
| ToastNoIngredient.png | 재료가 부족합니다. | 50px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 425×72 |
| ToastNoPlate.png | 깨끗한 접시가 없습니다. | 50px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 507×72 → **530×72 (폭 확대)** |
| ToastNoStock.png | 재고가 없습니다. | 50px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 386×72 |
| ToastNothingToComplete.png | 완성할 음료가 없습니다. | 50px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 490×72 → **531×72 (폭 확대)** |
| ToastOrderSkipped.png | 먼저 들어온 주문부터 서빙해 주세요. | 50px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 758×72 → **815×72 (폭 확대)** |
| ToastPlaceCup.png | 커피잔을 놓아 주세요. | 50px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 497×72 |
| ToastPlaceShotGlass.png | 빈 컵을 놓아 주세요. | 50px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 468×72 |
| ToastReturnScoop.png | 스쿱을 먼저 반납해 주세요. | 50px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 562×72 → **609×72 (폭 확대)** |
| ToastSetPortafilter.png | 포터필터를 먼저 세팅해 주세요. | 50px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 600×72 → **707×72 (폭 확대)** |
| ToastShotAlready.png | 이미 샷이 담긴 컵입니다. | 50px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 533×72 → **560×72 (폭 확대)** |
| ToastTampedOnly.png | 탬핑을 완료한 포터필터만 장착할 수 있습니다. | 50px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 837×72 → **1008×72 (폭 확대)** |
| ToastWaterFirst.png | 물을 먼저 받아 주세요. | 50px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 514×72 → **515×72 (폭 확대)** |
| BannerClosed.png | 영업 종료 | 145px | #FCF0CA → #EEAB31 | #5D2608 9px | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1800×440 |
| DiscardButton.png | 버리기 | 48px | #380F00 | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 380×124 |
| PromptKey.png | E | 20px | #380F00 | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 50×50 |
| PromptOpen.png | E | 24px | #380F00 | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 192×72 |
| PromptOpen.png | 열기 | 36px | #380F00 | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 192×72 |
| PromptUse.png | E | 24px | #380F00 | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 192×72 |
| PromptUse.png | 사용 | 36px | #380F00 | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 192×72 |

- 검수: 완성 PNG에서 한글 철자·공백·문장부호와 잘림을 눈으로 확인했다. 그림 포함 PNG는 지정 글자 영역 밖의 RGBA가 원본과 일치하고 알파·캔버스 크기가 동일한지 검사했다. 투명 라벨의 높이는 모두 유지했다. `.meta`, 코드, 씬, 프리팹, `.asset`, `docs/`는 수정하지 않았다. Unity에서 재임포트한 뒤 실제 표시 크기의 가독성과 라벨 폭 증가에 따른 배치를 확인해야 한다.
