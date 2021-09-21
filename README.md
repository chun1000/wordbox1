### 1. 프로젝트 개요

---

 ![image-20210921232018464](https://raw.githubusercontent.com/ChunYS/ImageRepo2021/Image/img/image-20210921232018464.png)

- 본 프로젝트는 암기할 영어 단어를 관리해주는 프로그램입니다. 모티브가 된 프로그램은 직접 유지보수하며 사용했던 위 사진의 WordMemory라는 프로그램입니다. 
- 해당 프로그램은 주변인들의 좋은 평가를 받아서 계속 업데이트 배포를 반복했는데, 덩치가 점점 커지고 유지보수가 불가능하다고 판단했습니다.
- 새로운 프로그램은 새로 배운 언어인 C#과 컴퓨터공학 과목인 SW분석 및 설계 방법을 적용해서 유지 보수가 용이하도록 하면서, 사용자가 사용하기 편리하도록 GUI를 도입하는 것을 목표로 하였습니다.

### 2. 특징 및 기능

---

##### 1) 단어 저장 및 자가 테스트

- 본 프로그램은 사용자가 직접 외우고 싶은 단어를 저장합니다. 직접 입력할수도 있고, 엑셀이나 메모장에 저장된 다량의 단어를 불러올 수도 있습니다. 저장된 단어들은 커스터마이징 된 규칙에 따라서 테스트에 등장합니다.

##### 2) 규칙 커스터마이징

- 본 프로그램의 가장 큰 특징입니다. WordBox라는 이름에 따라서 실제 단어 암기 카드와 상자를 이용해서 암기하는 방식을 FSM(Finite State Machine)의 형태를 따서 프로그래밍 하였습니다.

![image-20210921232807619](https://raw.githubusercontent.com/ChunYS/ImageRepo2021/Image/img/image-20210921232807619.png)

- 예를 들어서, 일주일에 3번 암기하는 방법이 있을 수 있다면, 데이터 파일에 위와 같은 형식을 띕니다.

![image-20210921232834976](https://raw.githubusercontent.com/ChunYS/ImageRepo2021/Image/img/image-20210921232834976.png)

- 해당 데이터 파일은 프로그램에서 위와 같이 취급됩니다.
- 예를 들어서, Apple이라는 단어가 등록됐을 때, 최초에는 0번 상자에 들어가게 됩니다. 0번 상자에 들어온 Apple은 시험에서 맞출 경우 1번 상자로 들어가며, 하루가 지나야만 시험에 등장합니다. 틀릴 경우 0번 상자로 다시 들어가며 1시간을 대기한 뒤 시험에 등장합니다. 1번 상자로 들어간 Apple을 또 한 번 맞추면 2번 상자로 들어가며, 6일 뒤에 재등장하고, 이를 다시 맞추면 종결점인 3번 상자로 들어가서 삭제됩니다.
- 모든 단어가 상자 단위로 관리가 되면서 맞출 때와 틀렸을 때 들어가는 상자를 마음대로 바꿀 수 있고, 상자를 최대 100개까지 늘려서 관리할 수 있습니다.

![image-20210921233052776](https://raw.githubusercontent.com/ChunYS/ImageRepo2021/Image/img/image-20210921233052776.png)

- 자신이 어학 시험을 목적으로 한다면 그만큼 짧고 촘촘한 주기의 룰을 적용가능하고, 개인적인 신문기사 공부를 목적으로 한다면 넉넉하고 느슨한 룰을 적용하는 등, 자신이 원하는 룰을 만들어서 암기할 영어 단어들을 관리할 수 있습니다.

### 3. 실제 동작

---

##### 1) 그룹 및 룰 관리 기능

![image-20210921233119915](https://raw.githubusercontent.com/ChunYS/ImageRepo2021/Image/img/image-20210921233119915.png)

- 그룹을 생성할 수 있으며, 그룹 하나에는 룰 하나를 선택해서 적용할 수 있습니다.

##### 2) 단어 등록 및 관리

![image-20210921233150042](https://raw.githubusercontent.com/ChunYS/ImageRepo2021/Image/img/image-20210921233150042.png)

- 하단의 단어 추가란의 텍스트 상자에 단어를 집어넣고 엔터를 치면, 단어가 리스트에 등록됩니다.

![image-20210921233212886](https://raw.githubusercontent.com/ChunYS/ImageRepo2021/Image/img/image-20210921233212886.png)

- 단어의 등장일과 상태, 메모 등이 같이 표시되면서 반복이 끝나서 삭제가 예정된 단어들은 적색으로 표시됩니다. 아직 등장일이 채워지지 않아서 시험에 등장하지 않는 단어들은 회색으로 표시됩니다.

![image-20210921233256703](https://raw.githubusercontent.com/ChunYS/ImageRepo2021/Image/img/image-20210921233256703.png)

- 해당 단어를 리스트에서 클릭하면 편집하거나 삭제가 가능합니다.

![image-20210921233318128](https://raw.githubusercontent.com/ChunYS/ImageRepo2021/Image/img/image-20210921233318128.png)

- 파일을 통해서 일괄적으로 여러 단어들을 불러올 수 있습니다.

##### 2) 시험 기능

![image-20210921233351540](https://raw.githubusercontent.com/ChunYS/ImageRepo2021/Image/img/image-20210921233351540.png)

- 시험은 영어를 토대로 뜻을 입력하는 의미 시험과, 한글을 통해서 영어를 입력하는 철자 시험 두 종류가 있습니다. 전자는 단축키(Y/N)혹은 버튼을 사용한 수동 채점이며, 후자는 자동 채점입니다.
- 현재 후자의 기능은 TTS를 테스트하기 위해서 테스트 코드를 삽입했기에 구현은 되어있지만 막혀 있습니다

![image-20210921233421179](https://raw.githubusercontent.com/ChunYS/ImageRepo2021/Image/img/image-20210921233421179.png)

- 재시험 기능을 키면, 복습창이 뜹니다. 단어 가림판을 쓰면 좌측의 단어들이 전부 가려지며, 뜻가림판을 쓰면 반대로 우측의 의미들이 전부 가려집니다. 
- 틀린 단어들만 다시 모아서 다 맞출 때까지 복습을 하게 되는데, 이때는 맞추고 틀리고 여부에 따라 단어의 상태가 변하지는 않습니다.

##### 3) 설정 기능

![image-20210921233448024](https://raw.githubusercontent.com/ChunYS/ImageRepo2021/Image/img/image-20210921233448024.png)

- 데이터를 저장할 경로를 설정합니다. 구글 드라이브와 연동해서 사용하기 위해서 만든 기능입니다. 
- 철자 시험은 그냥 \_\_\_\_\_로 표현하는 방식과 s\_\_\_y 처럼 첫 글자와 끝 글자 힌트를 줄 지 결정할 수 있습니다.

### 3. 기능 설계

---

- 해당 파트는 코드를 설계하기 전에 만든 프로그램 설계도에 기반하고 있습니다. 최신 버전의 프로그램과 일치하지 않는 부분이 있을 수 있습니다. 
- 이런 형태로 설계됐다는 것을 간략하게 보여주는 정도의 목적이기에, 전체가 아닌 몇 가지 기능들만 READEME에 넣었습니다.

##### 1) 그룹생성과 프로그램 시작

![image-20210921233622069](https://raw.githubusercontent.com/ChunYS/ImageRepo2021/Image/img/image-20210921233622069.png)

##### 2) 정답처리

![image-20210921233633093](https://raw.githubusercontent.com/ChunYS/ImageRepo2021/Image/img/image-20210921233633093.png)

##### 3) 시험 단어 고르기

![image-20210921233643775](https://raw.githubusercontent.com/ChunYS/ImageRepo2021/Image/img/image-20210921233643775.png)





##### 4) 정답여부 판별 및 갱신

![image-20210921233702497](https://raw.githubusercontent.com/ChunYS/ImageRepo2021/Image/img/image-20210921233702497.png)

